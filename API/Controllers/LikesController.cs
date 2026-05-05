using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController : BaseApiController
{
    private readonly IUnitOfWork _uow;

    public LikesController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpPost("{targetMemberId}")]
    public async Task<ActionResult> ToggleLike(string targetMemberId)
    {
        var sourceMemberId = User.GetMemberId();

        if (sourceMemberId == targetMemberId) return BadRequest("You cannot like yourself");

        var existingLike = await _uow.LikesRepository.GetMemberLike(sourceMemberId, targetMemberId);

        if(existingLike == null)
        {
            var like = new MemberLike
            {
                SourceMemberId = sourceMemberId,
                TargetMemberId = targetMemberId
            };

            _uow.LikesRepository.AddLike(like);
        }
        else
        {
            _uow.LikesRepository.DeleteLike(existingLike);
        }

        if(await _uow.Complete()) return Ok();

        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IReadOnlyList<MemberLike>>> GetCurrentMemberLikeIds()
    {
        var sourceMemberId = User.GetMemberId();
        var likes = await _uow.LikesRepository.GetCurrentMemberLikeIds(sourceMemberId);

        return Ok(likes);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MemberDto>>> GetMemberLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.MemberId = User.GetMemberId();
        var members = await _uow.LikesRepository.GetMemberLikes(likesParams);

        return Ok(members);
    }

}
