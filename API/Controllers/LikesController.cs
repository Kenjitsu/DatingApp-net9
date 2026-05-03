using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController : BaseApiController
{
    private readonly ILikesRepository _likesRepository;

    public LikesController(ILikesRepository likesRepository)
    {
        _likesRepository = likesRepository;
    }

    [HttpPost("{targetMemberId}")]
    public async Task<ActionResult> ToggleLike(string targetMemberId)
    {
        var sourceMemberId = User.GetMemberId();

        if (sourceMemberId == targetMemberId) return BadRequest("You cannot like yourself");

        var existingLike = await _likesRepository.GetMemberLike(sourceMemberId, targetMemberId);

        if(existingLike == null)
        {
            var like = new MemberLike
            {
                SourceMemberId = sourceMemberId,
                TargetMemberId = targetMemberId
            };

            _likesRepository.AddLike(like);
        }
        else
        {
            _likesRepository.DeleteLike(existingLike);
        }

        if(await _likesRepository.SaveAllChanges()) return Ok();

        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IReadOnlyList<MemberLike>>> GetCurrentMemberLikeIds()
    {
        var sourceMemberId = User.GetMemberId();
        var likes = await _likesRepository.GetCurrentMemberLikeIds(sourceMemberId);

        return Ok(likes);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MemberDto>>> GetMemberLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.MemberId = User.GetMemberId();
        var members = await _likesRepository.GetMemberLikes(likesParams);

        return Ok(members);
    }

}
