using System.Linq.Expressions;
using API.DTOs;
using API.Entities;

namespace API.Extensions.Projections;

public static class MessagesProjectionExtansions
{
    public static IQueryable<MessageDto> ToDtoProjection(this IQueryable<Message> messages)
    {
        return messages.Select(GetMessageDtoProjection());
    }

    private static Expression<Func<Message, MessageDto>> GetMessageDtoProjection()
    {
        return message => new MessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            SenderDisplayName = message.Sender.DisplayName,
            SenderImageUrl = message.Sender.ImageUrl,
            RecipientId = message.RecipientId,
            RecipientDisplayName = message.Recipient.DisplayName,
            RecipientImageUrl = message.Recipient.ImageUrl,
            Content = message.Content,
            DateRead = message.DateRead,
            MessageSent = message.MessageSent,
        };
    }
}
