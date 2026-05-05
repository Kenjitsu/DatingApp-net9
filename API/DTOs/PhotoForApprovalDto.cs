namespace API.DTOs;

public record PhotoForApprovalDto
{
    public int Id { get; init; }
    public required string Url { get; init; }
    public required string UserId { get; init; }
    public bool IsApproved { get; init; }

}
