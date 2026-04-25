using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        if (await EmailExist(registerDto.Email!))
        {
            return BadRequest("Email is taken");
        }

        using var hmac = new HMACSHA512();

        var user = registerDto.MapRegisterDtoToAppUser(passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!)), passwordSalt: hmac.Key );

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        var userDto = user.MapAppUserToUserDto(_tokenService);

        //return new UserDto { Username = registerDto.Username!, Token = _tokenService.CreateToken(user), KnownAs = user.KnownAs };
        return userDto;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            //.Include(p => p.Photos)
            .FirstOrDefaultAsync(x =>
                x.Email.ToLower() == loginDto.Email.ToLower());

        if (user == null) return Unauthorized("Invalid email address");

        var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }

        var userDto = user.MapAppUserToUserDto(_tokenService);

        return userDto;
    }

    private async Task<bool> EmailExist(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
