using AgroTechProject.Dtos.BookingDto;
using AgroTechProject.Dtos.DashBoardDto;
using AgroTechProject.Services.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AgroTechProject.Controller;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingController(IBookingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllBookingsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var booking = await _service.GetBookingByIdAsync(id);
        return booking == null ? NotFound() : Ok(booking);
    }

    [HttpPost]
    [Authorize(Roles = "User,Farmer")]
    public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
    {
        try
        {
            var result = await _service.CreateBookingAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteBookingAsync(id);
        return NoContent();
    }
    
    // [HttpPut("{id}/status")]
    // [Authorize(Roles = "Owner")]
    // public async Task<IActionResult> UpdateStatus(int id, [FromBody] ReservationStatusUpdateDto dto)
    // {
    //     try
    //     {
    //         await _service.UpdateBookingStatusAsync(id, dto.Status);
    //         return Ok(new { message = $"Booking status updated to {dto.Status}" });
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    // }
    
    [HttpGet("pending")]
    [Authorize(Roles = "Owner")] // Only owner can view pending bookings
    public async Task<IActionResult> GetPendingBookings()
    {
        var bookings = await _service.GetPendingBookingsAsync();
        return Ok(bookings);
    }
    
    [HttpPut("update-status")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> UpdateBookingStatus([FromBody] BookingStatusUpdateDto dto)
    {
        try
        {
            await _service.UpdateBookingStatusAsync(dto);
            return Ok(new { message = "Booking status updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpGet("admin-dashboard")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<AdminBookingOverviewDto>>> GetAdminDashboard()
    {
        var data = await _service.GetAdminDashboardDataAsync();
        return Ok(data);
    }

    [HttpDelete("admin-dashboard/delete/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBookingFromDashboard(int id)
    {
        await _service.DeleteBookingAsync(id);
        return NoContent();
    }

}
