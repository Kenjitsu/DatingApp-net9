using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public record RegisterDto
{
    [Required]
    public string? DisplayName { get; init; }

    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    //[Required]
    //public string? KnownAs { get; set; }

    //[Required]
    //public string? Gender { get; set; }

    //[Required]
    //public string? DateOfBirth { get; set; }

    //[Required]
    //public string? City { get; set; }

    //[Required]
    //public string? Country { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string? Password { get; init; }
}
