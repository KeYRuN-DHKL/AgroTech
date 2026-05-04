using AgroTechProject.Dtos.UserDto;
using AgroTechProject.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroTechProject.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService service)
    {
        _userService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
       return Ok(await _userService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchByFullName([FromQuery] string name)
    {
        var users = await _userService.SearchByFullNameAsync(name);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequestDto dto)
    {
        await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = dto.Email }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserRequestDto dto)
    {
        await _userService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}