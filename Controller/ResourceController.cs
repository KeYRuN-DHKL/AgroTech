using Microsoft.AspNetCore.Mvc;
using AgroTechProject.Model;
using AgroTechProject.Data;
using Microsoft.EntityFrameworkCore;

namespace ResourceController;

[ApiController]
[Route("api/[Controller]")]

public class ResourceController : ControllerBase
{
  private readonly AppDbContext _context;

  public ResourceController(AppDbContext context)
  {
    _context = context;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllResource()
  {
    var resources = await _context.Resources.ToListAsync();
    return Ok(resources);
  }

  [HttpGet("{id:int}")] 
  public async Task<IActionResult> GetResourceById(int id)
  {
    var resources = await _context.Resources.FindAsync(id);

    if (resources == null)
    {
      return NotFound(new { Message = $"Data with the id {id} not found..." });
    }
    
    return Ok(resources);
  }

  [HttpGet("byname")]
  public async Task<IActionResult> GetResourceByName([FromQuery] string name)
  {
    var resources = await _context.Resources
      .Where(r => r.Name.Contains(name))
      .ToListAsync();

    if (resources == null || !resources.Any())
    {
      return NotFound(new { Message = $"Data with the name {name} not found.." });
    }
    
    return Ok(resources);
  }

  [HttpPost("{id}")]
  public async Task<IActionResult> AddResourceById(int id,[FromBody] ResourceModel NewResource)
  {
    
    if (NewResource == null)
    {
      return BadRequest(new { Message = $"Cannot insert the resource with id {id}" });
    }

    await _context.Resources.AddAsync(NewResource);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(AddResourceById), new { id = NewResource.Id }, NewResource);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateResourceById(int id,[FromBody] ResourceModel NewResource)
  {
    var resource = await _context.Resources.FindAsync(id);

    if (resource == null)
    {
      return NotFound(new {Message = $"Cannot update data with id {id} "});
    }
    
    resource.Name = NewResource.Name;
    await _context.SaveChangesAsync();

    return Ok(new { Message = $"Data updated successfully with id {id}", resource });
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteResourceById(int id)
  {
    var resource = await _context.Resources.FindAsync(id);

    if (resource == null)
    {
      return NotFound(new { Message = "Cannot find resource with id: {id} " });
    }

    _context.Resources.Remove(resource);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}