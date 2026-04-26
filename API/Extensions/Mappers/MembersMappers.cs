using API.DTOs;
using API.Entities;
using API.Extensions.Utilities;
using System.Linq.Expressions;

namespace API.Extensions.Mappers;

public static class MembersMappers
{
    public static void MapMemberUpdateDtoToMember(this Member member, MemberUpdateDto memberUpdateDto)
    {
        member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
        member.Description = memberUpdateDto.Description ?? member.Description;
        member.City = memberUpdateDto.City ?? member.City;
        member.Country = memberUpdateDto.Country ?? member.Country;

        member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;
    }
}
