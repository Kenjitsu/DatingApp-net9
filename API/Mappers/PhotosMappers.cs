using API.DTOs;
using API.Entities;

namespace API.Mappers;

public static class PhotosMappers
{
    public static PhotoDto MapPhotoUploadToPhotoDto(this Photo photo)
    {
        var photoDto = new PhotoDto
        {
            Id = photo.Id,
            Url = photo.Url,
            IsMain = photo.IsMain,
        };

        return photoDto;
    }
}
