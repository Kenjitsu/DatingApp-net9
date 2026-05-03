using API.DTOs;
using API.Entities;
using API.Extensions.Projection;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class LikesRepository : ILikesRepository
{
    private readonly DataContext _dataContext;

    public LikesRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public void AddLike(MemberLike like)
    {
        _dataContext.Likes.Add(like);
    }

    public void DeleteLike(MemberLike like)
    {
        _dataContext.Likes.Remove(like);
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await _dataContext.Likes
            .Where(x => x.SourceMemberId == memberId)
            .Select(x => x.TargetMemberId)
            .ToListAsync();
    }

    public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
    {
        return await _dataContext.Likes
            .FindAsync(sourceMemberId, targetMemberId);
    }

    public async Task<IReadOnlyList<MemberDto>> GetMemberLikes(string predicate, string memberId)
    {
        var query = _dataContext.Likes.AsQueryable();

        switch (predicate)
        {
            case "liked":
                return await query.Where(x => x.SourceMemberId == memberId)
                    .Select(x => x.TargetMember)
                    .ProjectToMemberDtos()
                    .ToListAsync();
            case "likedBy":
                return await query.Where(x => x.TargetMemberId == memberId)
                    .Select(x => x.SourceMember)
                    .ProjectToMemberDtos()
                    .ToListAsync();
            default: //mutual
                var likedIds = await GetCurrentMemberLikeIds(memberId);

                return await query.Where(x => x.TargetMemberId == memberId && likedIds.Contains(x.SourceMemberId))
                    .Select(x => x.SourceMember)
                    .ProjectToMemberDtos()
                    .ToListAsync();
        }
    }

    public async Task<bool> SaveAllChanges()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }
}
