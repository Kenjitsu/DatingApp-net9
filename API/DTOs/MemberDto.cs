using API.Entities;

namespace API.DTOs;

public record MemberDto
{
    public required string Id { get; init; }
    public string? DisplayName { get; init; }
    public DateOnly DateOfBirth { get; set; }
    public string? ImageUrl { get; init; }
    //public string? KnownAs { get; init; }
    public DateTime Created { get; init; }
    public DateTime LastActive { get; init; }
    public string? Gender { get; init; }
    public string? Description { get; init; }
    //public string? Interests { get; init; }
    //public string? LookingFor { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    //public List<PhotoDto>? Photos { get; init; } = [];
}
