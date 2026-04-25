using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Repositories;

public interface IMemberRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    //Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<MemberDto?> GetByIdAsync(string id);

    Task<IReadOnlyList<PhotoDto>> GetPhotosForMemberAsync(string memberId);
    //Task<AppUser?> GetUserByUserNameAsync(string username);

    //Task<PagedList<MemberDto>> GetMembersAsync(MemberParams memberParams);
    Task<IReadOnlyList<MemberDto>> GetMembersAsync();
    //Task<MemberDto?> GetMemberAsync(string username);
}
