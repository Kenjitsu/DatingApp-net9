using API.DTOs;
using API.Entities;
using API.Extensions.Projections;
using API.Helpers;
using API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;

    public MessageRepository(DataContext context)
    {
        _context = context;
    }

    public void AddGroup(Group group)
    {
        _context.Groups.Add(group);
    }

    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await _context.Connections.FindAsync(connectionId);
    }

    public async Task<Group?> GetGroupForConnection(string connectionId)
    {
        return await _context.Groups
            .Include(g => g.Connections)
            .Where(g => g.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
    }

    public async Task<Message?> GetMessage(string messageId)
    {
        return await _context.Messages.FindAsync(messageId);
    }

    public async Task<Group?> GetMessageGroup(string groupName)
    {
        return await _context.Groups
            .Include(c => c.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName);
    }

    public async Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams)
    {
        var query = _context.Messages
           .OrderByDescending(m => m.MessageSent)
           .AsQueryable();

        query = messageParams.Container switch
        {
            "Outbox" => query.Where(x => x.SenderId == messageParams.MemberId && x.SenderDeleted == false),
            _ => query.Where(x => x.RecipientId == messageParams.MemberId && x.RecipientDeleted == false)
        };

        var messageQuery = query.ToDtoProjection();
        
        return await PaginationHelper.CreateAsync(messageQuery, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
    {
        await _context.Messages
            .Where(x => x.RecipientId == currentMemberId 
                    && x.SenderId == recipientId && x.DateRead == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.DateRead, DateTime.UtcNow));

        var messages = await _context.Messages
            .Where(x => (x.RecipientId == currentMemberId && x.RecipientDeleted == false && x.SenderId == recipientId)
                || (x.SenderId == currentMemberId && x.SenderDeleted == false && x.RecipientId == recipientId))
            .OrderBy(x => x.MessageSent)
            .ToDtoProjection()
            .ToListAsync();

        return messages;
    }

    public async Task RemoveConnection(string connectionId)
    {
        await _context.Connections
            .Where(c => c.ConnectionId == connectionId)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
