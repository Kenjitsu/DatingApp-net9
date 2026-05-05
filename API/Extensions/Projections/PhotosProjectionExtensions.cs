using System.Linq.Expressions;
using API.DTOs;
using API.Entities;

namespace API.Extensions.Projection;

public static class PhotosProjectionExtensions
{
    public static IQueryable<PhotoDto> ToDtoProjection(this IQueryable<Photo> photos)
    {
        return photos.Select(GetPhotoDtoProjection());
    }

    private static Expression<Func<Photo, PhotoDto>> GetPhotoDtoProjection()
    {
        return photo => new PhotoDto
        {
            Id = photo.Id,
            Url = photo.Url,
            MemberId = photo.MemberId,
            PublicId = photo.PublicId,
            IsApproved = photo.IsApproved,
        };
    }

    public static IQueryable<PhotoForApprovalDto> ToApprovalDtoProjection(this IQueryable<Photo> photos)
    {
        return photos.Select(GetPhotoForApprovalDtoProjection());
    }

    private static Expression<Func<Photo, PhotoForApprovalDto>> GetPhotoForApprovalDtoProjection()
    {
        return photo => new PhotoForApprovalDto
        {
            Id = photo.Id,
            Url = photo.Url,
            UserId = photo.MemberId,
            IsApproved = photo.IsApproved,
        };
    }

}
