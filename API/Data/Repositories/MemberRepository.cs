using API.DTOs;
using API.Entities;
using API.Extensions.Mappers;
using API.Extensions.Projection;
using API.Helpers;
using API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly DataContext _dataContext;

    public MemberRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<MemberDto?> GetByIdAsync(string id)
    {
        return await _dataContext.Members
            .Where(m => m.Id == id)
            .ProjectToMemberDtos()
            .SingleOrDefaultAsync();
    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        //return await _dataContext.Users
        //    .Where(x => x.UserName == username)
        //    .Select(MembersMappers.GetMemberDtoProjection())
        //    .SingleOrDefaultAsync();

        return null;
    }

    //public async Task<PagedList<MemberDto>> GetMembersAsync(MemberParams memberParams)
    //{
    //    var query = _dataContext.Members.ProjectToMemberDtos();

    //    return await PagedList<MemberDto>.CreateAsync(query, memberParams.PageNumber, memberParams.PageSize);
    //}

    public async Task<IReadOnlyList<MemberDto>> GetMembersAsync()
    {
        var query = _dataContext.Members.ProjectToMemberDtos();

        return await query.ToListAsync();
    }

    public async Task<Member?> GetMembeToUpdaterByIdAsync(string id)
    {
        var member = await _dataContext.Members
            .Include(x => x.User)
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(m => m.Id == id);

        if (member == null) return null;

        return member;
    }

    //public async Task<IReadOnlyList<Member>> GetMembersAsync()
    //{
    //    return await _dataContext.Members.ToListAsync();
    //}

    public async Task<bool> SaveAllAsync()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public void Update(Member member)
    {
        _dataContext.Entry(member).State = EntityState.Modified;
    }

    public async Task<IReadOnlyList<PhotoDto>> GetPhotosForMemberAsync(string memberId)
    {
        return await _dataContext.Members
            .Where(p => p.Id == memberId)
            .SelectMany(x => x.Photos)
            .ProjectToPhotoDtos()
            .ToListAsync();
    }
}
