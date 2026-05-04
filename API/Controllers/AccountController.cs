using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions.Mappers;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = registerDto.MapRegisterDtoToAppUser();

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        var userDto = await user.MapAppUserToUserDto(_tokenService);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("identity", error.Description);
            }

            return ValidationProblem();
        }

        await _userManager.AddToRoleAsync(user, "Member");

        await SetRefreshTokenCookie(user);

        return userDto;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized("Invalid email address");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid password");

        var userDto = await user.MapAppUserToUserDto(_tokenService);

        await SetRefreshTokenCookie(user);

        return userDto;
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<UserDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken)) return NoContent();

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken 
                && u.RefreshTokenExpiry > DateTime.UtcNow);

        if (user == null) return Unauthorized("Invalid refresh token");

        await SetRefreshTokenCookie(user);
        var userDto = await user.MapAppUserToUserDto(_tokenService);

        return userDto;
    }

    private async Task SetRefreshTokenCookie(AppUser user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = user.RefreshTokenExpiry
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
