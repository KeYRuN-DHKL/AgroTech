using AgroTechProject.Dtos.ReviewDto;
using AgroTechProject.Model;
using AgroTechProject.Repositories.ReviewRepo;

namespace AgroTechProject.Services.Review;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _repo;

    public ReviewService(IReviewRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ReviewReadDto>> GetAllAsync()
    {
        var reviews = await _repo.GetAllAsync();
        return reviews.Select(r => new ReviewReadDto
        {
            Id = r.Id,
            ResourceId = r.ResourceId,
            ResourceName = r.Resource.Name,
            FarmerId = r.FarmerId,
            FarmerName = r.Farmer.FullName,
            Rating = r.Rating,
            Comment = r.Comment
        });
    }

    public async Task<ReviewReadDto?> GetByIdAsync(int id)
    {
        var r = await _repo.GetByIdAsync(id);
        if (r == null) return null;

        return new ReviewReadDto
        {
            Id = r.Id,
            ResourceId = r.ResourceId,
            ResourceName = r.Resource.Name,
            FarmerId = r.FarmerId,
            FarmerName = r.Farmer.FullName,
            Rating = r.Rating,
            Comment = r.Comment
        };
    }

    public async Task<ReviewReadDto> CreateAsync(ReviewCreateDto dto)
    {
        var review = new ReviewModel
        {
            ResourceId = dto.ResourceId,
            FarmerId = dto.FarmerId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        await _repo.AddAsync(review);

        return new ReviewReadDto
        {
            Id = review.Id,
            ResourceId = review.ResourceId,
            FarmerId = review.FarmerId,
            Rating = review.Rating,
            Comment = review.Comment
        };
    }
}
