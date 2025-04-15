using API.DTOs;
using API.Entities;

namespace API.Mappers;

public static class PhotosMappers
{
    public static IEnumerable<PhotoDto> MapListOfPhotosToPhotosDto(this IEnumerable<Photo> photosList)
    {

        //if(photosList != null && photosList.Any())
        //{
        //    var photosDto = photosList.Select(photo => new PhotoDto
        //    {
        //        Id = photo.Id,
        //        IsMain = photo.IsMain,
        //        Url = photo.Url
        //    });

        //    return photosDto;
        //}
        //return Enumerable.Empty<PhotoDto>();

        return [];
    }
}
