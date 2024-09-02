using ChallengePoint.Domain.Interface;
using ChallengePoint.Domain.Models;
using ChallengePoint.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ChallengePoint.Infrastructure.Repositories
{
    public class CollaboratorRepository(AppDbContext appDbContext) : ICollaboratorRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task AddAsync(CollaboratorModel collaborator)
        {
            await _appDbContext.Collaborators.AddAsync(collaborator);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var collaborator = await _appDbContext.Collaborators.FindAsync(id);
            if (collaborator != null)
            {
                _appDbContext.Collaborators.Remove(collaborator);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CollaboratorModel>> GetAllAsync(int pageNumber, int pageQuantity)
        {
            return await _appDbContext.Collaborators
                .Skip((pageNumber - 1) * pageQuantity)
                .Take(pageQuantity)
                .Select(c => new CollaboratorModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Position = c.Position,
                    Salary = c.Salary,
                    Enrollment = c.Enrollment
                })
                .ToListAsync();
        }

        public async Task<CollaboratorModel?> GetByIdAsync(int id)
        {
            return await _appDbContext.Collaborators
                .Where(c => c.Id == id)
                .Select(c => new CollaboratorModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Position = c.Position,
                    Salary = c.Salary,
                    Enrollment = c.Enrollment
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CollaboratorModel?> GetByEnrollmentAsync(string enrollment)
        {
            return await _appDbContext.Collaborators
                .Where(c => c.Enrollment == enrollment)
                .Select(c => new CollaboratorModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Position = c.Position,
                    Salary = c.Salary,
                    Enrollment = c.Enrollment
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(CollaboratorModel collaborator)
        {
            _appDbContext.Collaborators.Update(collaborator);
            await _appDbContext.SaveChangesAsync();
        }

        public Task<bool> ExistsByEnrollmentAsync(string enrollment)
        {
            return _appDbContext.Collaborators.AnyAsync(c => c.Enrollment == enrollment);
        }
    }
}