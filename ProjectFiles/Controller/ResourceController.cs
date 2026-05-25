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
    [Authorize (Roles = "Owner,Admin")]
    public async Task<IActionResult> Create([FromForm] ResourceRequestDto dto)
    {
        try
        {
            await _resourceservice.AddAsync(dto);
            return Ok("Resource Added Successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize (Roles = "Owner,Admin")]
    public async Task<IActionResult> Update(int id,[FromForm] ResourceRequestDto dto)
    {
        try
        {
            await _resourceservice.UpdateAsync(id, dto);
            return Ok("Resource Updated Successfully");
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize (Roles = "Owner,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _resourceservice.DeleteAsync(id);
            return Ok("Resource deleted successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}