using System.Linq.Expressions;
using API.DTOs;
using API.Entities;

namespace API.Extensions.Utilities.Projection;

public static class PhotosProjectionExtensions
{
    public static IQueryable<PhotoDto> ProjectToPhotoDtos(this IQueryable<Photo> photos)
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
            //IsMain = photo.IsMain,
        };
    }

}
