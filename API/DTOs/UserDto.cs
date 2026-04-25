namespace API.DTOs;

public record UserDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string DisplayName { get; init; }
    //public required string KnownAs { get; init; }
    public string? ImageUrl { get; init; }
    public required string Token { get; init; }

}
