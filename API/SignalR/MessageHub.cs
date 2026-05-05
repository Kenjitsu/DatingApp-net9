using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Extensions.Mappers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<PresenceHub> _presenceHubContext;

    public MessageHub(IUnitOfWork uow, IHubContext<PresenceHub> presenceHubContext)
    {
        _uow = uow;
        _presenceHubContext = presenceHubContext;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request?.Query["userId"].ToString() ?? throw new HubException("User not found");

        var groupName = GetGroupName(GetUserId(), otherUser);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await AddToGroup(groupName);

        var messages = await _uow.MessageRepository.GetMessageThread(GetUserId(), otherUser);
        await Clients.Group(groupName).SendAsync("RecieveMessageThread", messages);
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var sender = await _uow.MemberRepository.GetMemberEntityByIdAsync(GetUserId());
        var recipient = await _uow.MemberRepository.GetMemberEntityByIdAsync(createMessageDto.RecipientId);

        if (sender == null || recipient == null || sender.Id == createMessageDto.RecipientId)
        {
            throw new HubException("Cannot send message");
        }

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = createMessageDto.Content,
        };

        var groupName = GetGroupName(sender.Id, recipient.Id);
        var group = await _uow.MessageRepository.GetMessageGroup(groupName);
        var userInGroup = group != null && group.Connections.Any(x => x.UserId == message.RecipientId);

        if (userInGroup)
        {
            message.DateRead = DateTime.UtcNow;
        }

        _uow.MessageRepository.AddMessage(message);

        if (await _uow.Complete())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", message.ToDto());
            var conneections = await PresenceTracker.GetConnectionsForUser(recipient.Id);

            if(conneections != null && conneections.Count > 0 && !userInGroup)
            {
                await _presenceHubContext.Clients.Clients(conneections).SendAsync("NewMessageReceived", message.ToDto());
            }
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _uow.MessageRepository.RemoveConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    private async Task<bool> AddToGroup(string groupName)
    {
        var group = await _uow.MessageRepository.GetMessageGroup(groupName);
        var connection = new Connection(Context.ConnectionId, GetUserId());

        if(group == null)
        {
            group = new Group(groupName);
            _uow.MessageRepository.AddGroup(group);
        }

        group.Connections.Add(connection);

        return await _uow.Complete();
    }

    private static string GetGroupName(string? caller, string? other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;

        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    private string GetUserId() => Context.User?.GetMemberId() ?? throw new HubException("Cannot get member id");

}
