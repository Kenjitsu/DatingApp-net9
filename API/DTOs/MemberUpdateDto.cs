namespace API.DTOs;

public record MemberUpdateDto
{
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
}
