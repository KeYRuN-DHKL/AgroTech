using AgroTechProject.Model;

namespace AgroTechProject.Repositories.ReviewRepo;

public interface IReviewRepository
{
    Task<IEnumerable<ReviewModel>> GetAllAsync();
    Task<ReviewModel?> GetByIdAsync(int id);
    Task AddAsync(ReviewModel review);
    Task UpdateAsync(ReviewModel review);
    Task DeleteAsync(ReviewModel review);
}
