using API.DTOs;
using API.Entities;
using API.Extensions.Utilities;
using System.Linq.Expressions;

namespace API.Mappers;

public static class MembersMappers
{
    public static IQueryable<MemberDto> ProjectAppUserListToMemberListDtos(this IQueryable<AppUser> userList)
    {
        var membersListDto = userList.Select(user => new MemberDto()
        {
            Id = user.Id,
            Username = user.UserName,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.Photos == null
                ? null
                : user.Photos
                .Where(p => p.IsMain)
                .Select(p => p.Url)
                .FirstOrDefault(),
            KnownAs = user.KnownAs,
            CreatedAt = user.CreatedAt,
            LastActive = user.LastActive,
            Gender = user.Gender,
            Introduction = user.Introduction,
            Interests = user.Interests,
            LookingFor = user.LookingFor,
            City = user.City,
            Country = user.Country,
            Photos = user.Photos == null
                ? new List<PhotoDto>()
                : user.Photos.Select(photo => new PhotoDto
                {
                    Id = photo.Id,
                    IsMain = photo.IsMain,
                    Url = photo.Url
                }).ToList()
            });

        return membersListDto;
    }

    public static IQueryable<MemberDto> ProjectToMemberDtos(this IQueryable<AppUser> users)
    {
        return users.Select(GetMemberDtoProjection());
    }


    public static Expression<Func<AppUser, MemberDto>> GetMemberDtoProjection()
    {
        return user => new MemberDto
        {
            Id = user.Id,
            Username = user.UserName,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.Photos == null
                ? null
                : user.Photos
                .Where(p => p.IsMain)
                .Select(p => p.Url)
                .FirstOrDefault(),
            KnownAs = user.KnownAs,
            CreatedAt = user.CreatedAt,
            LastActive = user.LastActive,
            Gender = user.Gender,
            Introduction = user.Introduction,
            Interests = user.Interests,
            LookingFor = user.LookingFor,
            City = user.City,
            Country = user.Country,
            Photos = user.Photos == null
                ? new List<PhotoDto>()
                : user.Photos.Select(photo => new PhotoDto
            {
                Id = photo.Id,
                IsMain = photo.IsMain,
                Url = photo.Url
            }).ToList()
        };
    }

    

    //private static string? GetPhotoUrl(IEnumerable<Photo> photosList)
    //{
    //    if (photosList != null && photosList.Any())
    //    {
    //        return photosList.FirstOrDefault(x => x.IsMain)!.Url ?? null;
    //    }

    //    return null;
    //}
}
