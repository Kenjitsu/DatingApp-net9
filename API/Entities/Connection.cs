namespace API.Entities;

public class Connection
{
    public Connection(string connectionId, string userId)
    {
        ConnectionId = connectionId;
        UserId = userId;
    }

    public string ConnectionId { get; set; }
    public string UserId { get; set; }

    // nav property
    public Group Group { get; set; } = null!;
}
