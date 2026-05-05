using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Extensions.Mappers;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessagesController : BaseApiController
{
    private readonly IUnitOfWork _uow;

    public MessagesController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var sender = await _uow.MemberRepository.GetMemberEntityByIdAsync(User.GetMemberId());
        var recipient = await _uow.MemberRepository.GetMemberEntityByIdAsync(createMessageDto.RecipientId);

        if (sender == null || recipient == null || sender.Id == createMessageDto.RecipientId)
        {
            return BadRequest("Cannot send this message.");
        }

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            Content = createMessageDto.Content,
        };

        _uow.MessageRepository.AddMessage(message);

        if (await _uow.Complete()) return message.ToDto();

        return BadRequest("Failed to send message.");
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<MessageDto>>> GetMessagesByContainer([FromQuery] MessageParams messageParams)
    {
        messageParams.MemberId = User.GetMemberId();

        return await _uow.MessageRepository.GetMessagesForMember(messageParams);
    }

    [HttpGet("thread/{recipientId}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(string recipientId)
    {
        var currentMemberId = User.GetMemberId();
        var messageThread = await _uow.MessageRepository.GetMessageThread(currentMemberId, recipientId);

        return Ok(messageThread);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(string id)
    {
        var memberId = User.GetMemberId();

        var message = await _uow.MessageRepository.GetMessage(id);

        if (message == null) return BadRequest("Cannot delete this message.");

        if(message.SenderId != memberId && message.RecipientId != memberId) return BadRequest("You cannot delete this message.");

        if (message.SenderId == memberId) message.SenderDeleted = true;
        if (message.RecipientId == memberId) message.RecipientDeleted = true;

        if(message is { SenderDeleted: true, RecipientDeleted: true })
        {
            _uow.MessageRepository.DeleteMessage(message);
        }

        if (await _uow.Complete()) return Ok();

        return BadRequest("Problem deleting message.");
    }


}
