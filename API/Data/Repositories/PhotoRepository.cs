using API.DTOs;
using API.Entities;
using API.Extensions.Projection;
using API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly DataContext _dataContext;

    public PhotoRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Photo?> GetPhotoById(int Id)
    {
        return await _dataContext.Photos
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(p => p.Id == Id);
    }

    public async Task<IReadOnlyList<PhotoForApprovalDto>> GetUnapprovedPhotos()
    {
        var photos = await _dataContext.Photos
            .IgnoreQueryFilters()
            .Where(p => !p.IsApproved)
            .ToApprovalDtoProjection()
            .ToListAsync();

        return photos;
    }

    public void RemovePhoto(Photo photo)
    {
        _dataContext.Photos.Remove(photo);
    }
}
