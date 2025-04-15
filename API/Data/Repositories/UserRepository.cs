using API.DTOs;
using API.Entities;
using API.Interfaces.Repositories;
using API.Mappers;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        return await _dataContext.Users.FindAsync(id);
    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await _dataContext.Users
            .Where(x => x.UserName == username)
            .Select(MembersMappers.GetMemberDtoProjection())
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<MemberDto>?> GetAllMembersAsync()
    {
        return await _dataContext.Users
            .ProjectToMemberDtos()
            .ToListAsync();
    }

    public async Task<AppUser?> GetUserByUserName(string username)
    {
        return await _dataContext.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _dataContext.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
    }
}
