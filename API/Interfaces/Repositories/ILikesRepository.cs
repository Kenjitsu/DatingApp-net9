using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Repositories;

public interface ILikesRepository
{
    Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId);
    Task<PaginatedResult<MemberDto>> GetMemberLikes(LikesParams likesParams);
    Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId);
    void DeleteLike(MemberLike like);
    void AddLike(MemberLike like);
    Task<bool> SaveAllChanges();
}
