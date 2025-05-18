using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Interfaces.Repositories;
using API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _photoService = photoService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetAllMembersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await _userRepository.GetMemberAsync(username);

        if (user == null)
            return NotFound();

        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

        if (user == null)
            return NotFound("Could not find user.");

        memberUpdateDto.MapMemberUpdateDtoToAppUser(user);

        if (await _userRepository.SaveAllAsync()) 
            return NoContent();

        return BadRequest("Failed to update the user.");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

        if (user == null)
            return BadRequest("Cannot update user photo.");

        var result = await _photoService.AddPhotoAsync(file);

        if(result.Error != null)
            return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, photo.MapPhotoUploadToPhotoDto());

        return BadRequest("Problem adding photo");
    }


    //[HttpGet("{id:int}")]
    //public async Task<ActionResult<AppUser>> GetUser(int id)
    //{
    //    var user = await _userRepository.GetByIdAsync(id);

    //    if (user == null) 
    //        return NotFound();

    //    return user;
    //}
}
