using AgroTechProject.Dtos.ReviewDto;

namespace AgroTechProject.Services.Review;

public interface IReviewService
{
    Task<IEnumerable<ReviewReadDto>> GetAllAsync();
    Task<ReviewReadDto?> GetByIdAsync(int id);
    Task<ReviewReadDto> CreateAsync(ReviewCreateDto dto);
}
