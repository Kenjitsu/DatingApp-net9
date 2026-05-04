using API.DTOs;
using API.Entities;
using API.Extensions.Projection;
using API.Helpers;
using API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class LikesRepository : ILikesRepository
{
    private readonly UserManager _dataContext;

    public LikesRepository(UserManager dataContext)
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

    public async Task<PaginatedResult<MemberDto>> GetMemberLikes(LikesParams likesParams)
    {
        var query = _dataContext.Likes.AsQueryable();
        IQueryable<MemberDto> result;

        switch (likesParams.Predicate)
        {
            case "liked":
                result = query.Where(x => x.SourceMemberId == likesParams.MemberId)
                    .Select(x => x.TargetMember)
                    .ToDtoProjection();
                break;
            case "likedBy":
                result = query.Where(x => x.TargetMemberId == likesParams.MemberId)
                    .Select(x => x.SourceMember)
                    .ToDtoProjection();
                break;
            default: //mutual
                var likedIds = await GetCurrentMemberLikeIds(likesParams.MemberId);

                result = query.Where(x => x.TargetMemberId == likesParams.MemberId && likedIds.Contains(x.SourceMemberId))
                    .Select(x => x.SourceMember)
                    .ToDtoProjection();
                break;
        }

        return await PaginationHelper.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<bool> SaveAllChanges()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }
}
