using AgroTechProject.Dtos.ResourceDto;
using AgroTechProject.Services.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroTechProject.Controller;

[ApiController]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    private readonly IResourceService _resourceservice;

    public ResourceController(IResourceService resourceservice)
    {
        _resourceservice = resourceservice;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        return Ok(await _resourceservice.GetAllAsync());
    }
       

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var resource = await _resourceservice.GetByIdAsync(id);
        if (resource == null) return NotFound();
        return Ok(resource);
    }

    [HttpPost]
    [Authorize (Roles = "Owner")]
    public async Task<IActionResult> Create(ResourceRequestDto dto)
    {
        await _resourceservice.AddAsync(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize (Roles = "Owner")]
    public async Task<IActionResult> Update(int id, ResourceRequestDto dto)
    {
        await _resourceservice.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize (Roles = "Owner")]
    public async Task<IActionResult> Delete(int id)
    {
        await _resourceservice.DeleteAsync(id);
        return Ok();
    }
}