using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Mappers;

public static class AppUserMappers
{
    public static AppUser MapRegisterDtoToAppUser(this RegisterDto registerDto, byte[] passwordHash, byte[] passwordSalt)
    {
        var appUser = new AppUser
        {
            DisplayName = registerDto.DisplayName!,
            //KnownAs = registerDto.KnownAs!,
            //Gender = registerDto.Gender!,
            //DateOfBirth = DateOnly.Parse(registerDto.DateOfBirth!),
            //City = registerDto.City!,
            //Country = registerDto.Country!,
            Email = registerDto.Email!,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        return appUser;

    }

    public static UserDto MapAppUserToUserDto(this AppUser user, ITokenService tokenService)
    {
        var userDto = new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = tokenService.CreateToken(user),
        };

        return userDto;

    }
}
