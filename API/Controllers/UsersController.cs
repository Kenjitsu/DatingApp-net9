using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    //[HttpGet("{id:int}")]
    //public async Task<ActionResult<AppUser>> GetUser(int id)
    //{
    //    var user = await _userRepository.GetByIdAsync(id);

    //    if (user == null) 
    //        return NotFound();

    //    return user;
    //}
}
