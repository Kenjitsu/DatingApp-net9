using System.Linq.Expressions;
using API.DTOs;
using API.Entities;

namespace API.Extensions.Mappers;

public static class PhotosMappers
{
    public static PhotoDto MapPhotoUploadToPhotoDto(this Photo photo)
    {
        var photoDto = new PhotoDto
        {
            Id = photo.Id,
            Url = photo.Url,
            MemberId = photo.MemberId,
            PublicId = photo.PublicId,
            //IsMain = photo.IsMain,
        };

        return photoDto;
    }

}
