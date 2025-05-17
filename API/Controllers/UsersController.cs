using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces.Repositories;
using API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null)
            return NotFound("No user found in token.");

        var user = await _userRepository.GetUserByUserNameAsync(username);

        if (user == null)
            return NotFound("Could not find user.");

        memberUpdateDto.MapMemberUpdateDtoToAppUser(user);

        if (await _userRepository.SaveAllAsync()) 
            return NoContent();

        return BadRequest("Failed to update the user.");
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
