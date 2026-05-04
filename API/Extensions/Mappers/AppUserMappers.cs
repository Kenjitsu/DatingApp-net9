using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Extensions.Mappers;

public static class AppUserMappers
{
    public static AppUser MapRegisterDtoToAppUser(this RegisterDto registerDto)
    {
        var appUser = new AppUser
        {
            DisplayName = registerDto.DisplayName!,
            Email = registerDto.Email!,
            UserName = registerDto.Email!,
            Member = new Member
            {
                DisplayName = registerDto.DisplayName,
                Gender = registerDto.Gender,
                DateOfBirth = DateOnly.Parse(registerDto.DateOfBirth),
                City = registerDto.City!,
                Country = registerDto.Country!,
            }
        };

        return appUser;

    }

    public static async Task<UserDto> MapAppUserToUserDto(this AppUser user, ITokenService tokenService)
    {
        var userDto = new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email!,
            ImageUrl = user.ImageUrl,
            Token = await tokenService.CreateToken(user),
        };

        return userDto;

    }
}
