using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.DTOs;
using RestaurantManager.Models;
using RestaurantManager.Models.Constants;
using RestaurantManager.Services;

namespace RestaurantManager.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserService _userService;

    public UserController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public IActionResult Login(Credentials creds)
    {
        var token = _userService.Login(creds);
        if (token == "Invalid username or password")
            return BadRequest(new { message = token });
        return Ok(token);
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpPost]
    public IActionResult Create(User user)
    {
        try
        {
            _userService.Create(user);
            return Created(nameof(Create), user);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _userService.Delete(id);
        if (!success)
            return NotFound("User not found");
        return Ok("User deleted successfully");
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UserDto userParam)
    {
        var user = _userService.GetById(id);

        if (user == null)
            return NotFound("User not found");

        if (userParam.Email != user.Email)
        {
            if (_userService.GetAll().Any(x => x.Email == userParam.Email))
                return BadRequest(new { message = "Username " + userParam.Email + " is already taken" });
        }

        user = _mapper.Map<UserDto, User>(userParam, user);
        try
        {
            _userService.Update(user);
            return Ok("User updated successfully");
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}
