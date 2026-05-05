using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _uow;
    private readonly IPhotoService _photoService;

    public AdminController(UserManager<AppUser> userManager, IUnitOfWork uow, IPhotoService photoService)
    {
        _userManager = userManager;
        _uow = uow;
        _photoService = photoService;
    }


    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await _userManager.Users.ToListAsync();
        var userList = new List<object>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userList.Add(new
            {
                user.Id,
                user.Email,
                Roles = roles.ToList()
            });
        }

        return Ok(userList);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{userId}")]
    public async Task<ActionResult<IList<string>>> EditRoles(string userId, [FromQuery] string roles)
    {
        if(string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role.");

        var selectedRoles = roles.Split(',').ToArray();
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return NotFound("Could not find user.");

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if(!result.Succeeded) return BadRequest("Failed to add to roles.");

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if(!result.Succeeded) return BadRequest("Failed to remove from roles.");

        return Ok(await _userManager.GetRolesAsync(user));
    }


    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public async Task<ActionResult<IReadOnlyList<PhotoForApprovalDto>>> GetPhotosForModeration()
    {
        var photos = await _uow.PhotoRepository.GetUnapprovedPhotos();
        return Ok(photos);
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("approve-photo/{id:int}")]
    public async Task<ActionResult> ApprovePhoto(int Id)
    {
        var photo = await _uow.PhotoRepository.GetPhotoById(Id);

        if(photo == null) return NotFound("Could not find photo.");

        var member = await _uow.MemberRepository.GetMemberToUpdateByIdAsync(photo.MemberId);

        if(member == null) return NotFound("Could not find member.");

        photo.IsApproved = true;

        if (member.ImageUrl == null)
        {
            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;
        }

        if (_uow.HasChanges())
        {
            if (await _uow.Complete()) return NoContent();
        }

        return BadRequest("Failed to update photo approval status.");
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPost("reject-photo/{id:int}")]
    public async Task<ActionResult> RejectPhoto(int Id)
    {
        var photo = await _uow.PhotoRepository.GetPhotoById(Id);

        if (photo == null) return NotFound("Could not find photo.");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);

            _uow.PhotoRepository.RemovePhoto(photo);
        }
        else
        {
            _uow.PhotoRepository.RemovePhoto(photo);
        }

        if (_uow.HasChanges())
        {
            if (await _uow.Complete()) return NoContent();
        }

        return BadRequest("Failed to update photo approval status.");
    }
}
