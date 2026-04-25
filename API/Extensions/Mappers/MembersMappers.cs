using API.DTOs;
using API.Entities;
using API.Extensions.Utilities;
using System.Linq.Expressions;

namespace API.Extensions.Mappers;

public static class MembersMappers
{
    public static void MapMemberUpdateDtoToAppUser(this MemberUpdateDto memberUpdateDto, AppUser appUser)
    {
        //appUser.Introduction = memberUpdateDto.Introduction;
        //appUser.LookingFor = memberUpdateDto.LookingFor;
        //appUser.Interests = memberUpdateDto.Interests;
        //appUser.City = memberUpdateDto.City ?? appUser.City;
        //appUser.Country = memberUpdateDto.Country ?? appUser.Country;
    }
}
