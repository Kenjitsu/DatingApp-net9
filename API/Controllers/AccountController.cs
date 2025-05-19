using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username!))
        {
            return BadRequest("Username is taken");
        }

        using var hmac = new HMACSHA512();

        var user = registerDto.MapRegisterDtoToAppUser();

        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!));
        user.PasswordSalt = hmac.Key;

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return new UserDto { Username = registerDto.Username!, Token = _tokenService.CreateToken(user), KnownAs = user.KnownAs };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(x =>
                x.UserName == loginDto.Username.ToLower());

        if(user == null) return Unauthorized("Invalid username");

        var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}
