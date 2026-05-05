using System.Linq.Expressions;
using API.DTOs;
using API.Entities;

namespace API.Extensions.Projection;

public static class MembersProjectionExtensions
{
    public static IQueryable<MemberDto> ToDtoProjection(this IQueryable<Member> members)
    {
        return members.Select(GetMemberDtoProjection());
    }

    private static Expression<Func<Member, MemberDto>> GetMemberDtoProjection()
    {
        return member => new MemberDto
        {
            Id = member.Id,
            DateOfBirth = member.DateOfBirth,
            ImageUrl = member.ImageUrl,
            DisplayName = member.DisplayName,
            Created = member.Created,
            LastActive = member.LastActive,
            Gender = member.Gender,
            Description = member.Description,
            City = member.City,
            Country = member.Country,
            //Url = member.Photos == null
            //    ? null
            //    : member.Photos
            //    .Where(p => p.IsMain)
            //    .Select(p => p.Url)
            //    .FirstOrDefault(),
        };
    }
}
