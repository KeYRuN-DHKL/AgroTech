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
        try
        {
            await _userService.UpdateAsync(id, dto);
            return NoContent();
        }catch(InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Reset-Password")]
    public async Task<IActionResult> UpdatePassword([FromQuery] string email,[FromQuery] string password)
    {
        try
        {
            await _userService.ForgotPasswordAsync(email, password);
            return Ok("Password Reset Successfully...");
        }
        catch(InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }    
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }catch(InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}