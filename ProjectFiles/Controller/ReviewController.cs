using AgroTechProject.Dtos.ReviewDto;
using AgroTechProject.Services.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroTechProject.Controller;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    [Authorize(Roles = "Farmer,Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _reviewService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Farmer,Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _reviewService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Farmer,Admin")]
    public async Task<IActionResult> Create(ReviewCreateDto dto)
    {
        var result = await _reviewService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
