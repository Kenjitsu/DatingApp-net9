using API.DTOs;
using API.Entities;
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

    public async Task<PaginatedResult<MemberDto>> GetMembersAsync(MemberParams memberParams)
    {
        var query = _dataContext.Members.ProjectToMemberDtos();

        query = query.Where(m => m.Id != memberParams.CurrentMemberId);

        if(memberParams.Gender != null)
        {
            query = query.Where(m => m.Gender == memberParams.Gender);
        }

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MinAge));

        query = query.Where(m => m.DateOfBirth >=  minDob && m.DateOfBirth <= maxDob);

        query = memberParams.OrderBy switch
        {
            "created" => query.OrderByDescending(m => m.Created),
            _ => query.OrderByDescending(m => m.LastActive)
        };

        return await PaginationHelper.CreateAsync(query, memberParams.PageNumber, memberParams.PageSize);
    }

    public async Task<Member?> GetMemberToUpdateByIdAsync(string id)
    {
        var member = await _dataContext.Members
            .Include(x => x.User)
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(m => m.Id == id);

        if (member == null) return null;

        return member;
    }

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
