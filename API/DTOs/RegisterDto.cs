using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public record RegisterDto
{
    [Required]
    public string DisplayName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; init; } = string.Empty;

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public string DateOfBirth { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;
}
