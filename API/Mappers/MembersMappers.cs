﻿using API.DTOs;
using API.Entities;
using API.Extensions.Utilities;
using System.Linq.Expressions;

namespace API.Mappers;

public static class MembersMappers
{
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

    public static void MapMemberUpdateDtoToAppUser(this MemberUpdateDto memberUpdateDto, AppUser appUser)
    {
        appUser.Introduction = memberUpdateDto.Introduction;
        appUser.LookingFor = memberUpdateDto.LookingFor;
        appUser.Interests = memberUpdateDto.Interests;
        appUser.City = memberUpdateDto.City ?? appUser.City;
        appUser.Country = memberUpdateDto.Country ?? appUser.Country;
    }
}
