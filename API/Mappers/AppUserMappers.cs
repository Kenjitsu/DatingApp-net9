using API.DTOs;
using API.Entities;

namespace API.Mappers;

public static class AppUserMappers
{
    public static AppUser MapRegisterDtoToAppUser(this RegisterDto registerDto)
    {
        var appUser = new AppUser
        {
            UserName = registerDto.Username!.ToLower(),
            KnownAs = registerDto.KnownAs!,
            Gender = registerDto.Gender!,
            DateOfBirth = DateOnly.Parse(registerDto.DateOfBirth!),
            City = registerDto.City!,
            Country = registerDto.Country!,
        };

        return appUser;
    }
}
