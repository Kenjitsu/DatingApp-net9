using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Repositories;

public interface IMemberRepository
{
    void Update(Member member);
    Task<Member?> GetMemberEntityByIdAsync(string id);
    Task<MemberDto?> GetByIdAsync(string id);

    Task<IReadOnlyList<PhotoDto>> GetPhotosForMemberAsync(string memberId, bool isCurrentUser);
    Task<Member?> GetMemberToUpdateByIdAsync(string id);

    Task<PaginatedResult<MemberDto>> GetMembersAsync(MemberParams memberParams);
}
