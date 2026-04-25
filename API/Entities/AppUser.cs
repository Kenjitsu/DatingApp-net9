using API.Extensions.Utilities;

namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? ImageUrl { get; set; }

    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];

    //Navigation property
    public Member Member { get; set; } = null!;



    //public int GetAge()
    //{
    //    return DateOfBrith.CalculateAge();
    //}
}
