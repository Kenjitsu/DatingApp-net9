using API.DTOs;
using API.Entities;

namespace API.Interfaces.Repositories;

public interface IPhotoRepository
{
    Task<IReadOnlyList<PhotoForApprovalDto>> GetUnapprovedPhotos();
    Task<Photo?> GetPhotoById(int Id);
    void RemovePhoto(Photo photo);
}
