using API.DTOs;
using API.Entities;

namespace API.Interfaces.Repositories;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetByIdAsync(int id);
    Task<AppUser?> GetUserByUserNameAsync(string username);

    Task<IEnumerable<MemberDto>?> GetAllMembersAsync();
    Task<MemberDto?> GetMemberAsync(string username);
}
