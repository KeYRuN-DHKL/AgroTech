using AgroTechProject.Dtos.AuthDto;
using AgroTechProject.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroTechProject.Controller;
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (result == null) return BadRequest("User already exists.");
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null) return Unauthorized("Invalid credentials.");
        return Ok(result);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
    {
        var result = await _authService.RefreshTokenAsync(tokenDto);
        if (result == null) return Unauthorized("Invalid refresh token or expired.");
        return Ok(result);
    }

}