using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Extensions.Mappers;
using API.Helpers;
using API.Interfaces.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessagesController : BaseApiController
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMemberRepository _memberRepository;

    public MessagesController(IMessageRepository messageRepository, IMemberRepository memberRepository)
    {
        _messageRepository = messageRepository;
        _memberRepository = memberRepository;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var sender = await _memberRepository.GetMemberEntityByIdAsync(User.GetMemberId());
        var recipient = await _memberRepository.GetMemberEntityByIdAsync(createMessageDto.RecipientId);

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

        _messageRepository.AddMessage(message);

        if (await _messageRepository.SaveAllAsync()) return message.ToDto();

        return BadRequest("Failed to send message.");
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<MessageDto>>> GetMessagesByContainer([FromQuery] MessageParams messageParams)
    {
        messageParams.MemberId = User.GetMemberId();

        return await _messageRepository.GetMessagesForMember(messageParams);
    }

    [HttpGet("thread/{recipientId}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetMessageThread(string recipientId)
    {
        var currentMemberId = User.GetMemberId();
        var messageThread = await _messageRepository.GetMessageThread(currentMemberId, recipientId);

        return Ok(messageThread);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(string id)
    {
        var memberId = User.GetMemberId();

        var message = await _messageRepository.GetMessage(id);

        if (message == null) return BadRequest("Cannot delete this message.");

        if(message.SenderId != memberId && message.RecipientId != memberId) return BadRequest("You cannot delete this message.");

        if (message.SenderId == memberId) message.SenderDeleted = true;
        if (message.RecipientId == memberId) message.RecipientDeleted = true;

        if(message is { SenderDeleted: true, RecipientDeleted: true })
        {
            _messageRepository.DeleteMessage(message);
        }

        if (await _messageRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting message.");
    }


}
