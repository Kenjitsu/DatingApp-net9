using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public record LoginDto
{
    [Required]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}
