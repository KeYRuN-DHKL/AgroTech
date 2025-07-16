using AgroTechProject.Data;
using AgroTechProject.Model;
using Microsoft.EntityFrameworkCore;

namespace AgroTechProject.Repositories.ReviewRepo;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReviewModel>> GetAllAsync() =>
        await _context.Reviews
            .Include(r => r.Farmer)
            .Include(r => r.Resource)
            .ToListAsync();

    public async Task<ReviewModel?> GetByIdAsync(int id) =>
        await _context.Reviews
            .Include(r => r.Farmer)
            .Include(r => r.Resource)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task AddAsync(ReviewModel review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReviewModel review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ReviewModel review)
    {
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}
