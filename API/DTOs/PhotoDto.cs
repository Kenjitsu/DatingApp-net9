namespace API.DTOs;

public record PhotoDto
{
    public int Id { get; init; }
    public string? Url { get; init; }
    public string? PublicId { get; init; }
    public required string MemberId { get; init; }
    public bool IsApproved { get; init; }
    //public bool IsMain { get; init; }
}