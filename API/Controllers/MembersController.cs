using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Repositories;
using API.Extensions.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using API.Extensions;

namespace API.Controllers;

[Authorize]
public class MembersController : BaseApiController
{
    private readonly IMemberRepository _memberRepository;
    private readonly IPhotoService _photoService;

    public MembersController(IMemberRepository memberRepository, IPhotoService photoService)
    {
        _memberRepository = memberRepository;
        _photoService = photoService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(/*[FromQuery] MemberParams userParams*/)
    {
        var users = await _memberRepository.GetMembersAsync();

        //Response.AddPaginationHeader(users);

        return Ok(users);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
    {
        var memberId = User.GetMemberId();

        var member = await _memberRepository.GetMembeToUpdaterByIdAsync(memberId);

        if (member == null)
            return NotFound("Could not find member.");

        member.MapMemberUpdateDtoToMember(memberUpdateDto);

        if (await _memberRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update the member.");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember(string id)
    {
        var user = await _memberRepository.GetByIdAsync(id);

        if (user == null) return NotFound();

        return user;
    }

    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IEnumerable<Photo>>> GetMemberPhotos(string id)
    {
        var photos = await _memberRepository.GetPhotosForMemberAsync(id);

        if (photos == null) return NotFound();

        return Ok(photos);
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var member = await _memberRepository.GetMembeToUpdaterByIdAsync(User.GetMemberId());

        if (member == null)
            return BadRequest("Cannot update member photo.");

        var result = await _photoService.UploadPhotoAsync(file);

        if (result.Error != null)
            return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            MemberId = User.GetMemberId(),
        };

        if(member.ImageUrl == null)
        {
            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;
        }

        member.Photos.Add(photo);

        if (await _memberRepository.SaveAllAsync())
            return photo.MapPhotoUploadToPhotoDto();

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        //var member = await _userRepository.GetMembeToUpdaterByIdAsync(User.GetUserName());

        //if(member == null) return BadRequest("Could not find member.");

        //var photo = member.Photos.FirstOrDefault(x => x.Id == photoId);

        //if(photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo.");

        //var currentMain = member.Photos.FirstOrDefault(x => x.IsMain);
        //if(currentMain != null) currentMain.IsMain = false;
        //photo.IsMain = true;

        //if(await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        //var member = await _userRepository.GetMembeToUpdaterByIdAsync(User.GetUserName());

        //if(member == null) return BadRequest("Could not find member.");

        //var photo = member.Photos.FirstOrDefault(x => x.Id == photoId);

        //if(photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted.");

        //if(photo.PublicId != null)
        //{
        //    var result = await _photoService.DeletePhotoAsync(photo.PublicId);
        //    if(result.Error != null) return BadRequest(result.Error.Message);
        //}

        //member.Photos.Remove(photo);

        //if(await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo.");

    }


    
}
