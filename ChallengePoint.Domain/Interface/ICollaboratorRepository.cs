using ChallengePoint.Domain.Models;

namespace ChallengePoint.Domain.Interface
{
    public interface ICollaboratorRepository
    {
        Task<IEnumerable<CollaboratorModel>> GetAllAsync(int pageNumber, int pageQuantity);
        Task<CollaboratorModel?> GetByIdAsync(int id);
        Task AddAsync(CollaboratorModel colaborador);
        Task UpdateAsync(CollaboratorModel colaborador);
        Task DeleteAsync(int id);
        Task<CollaboratorModel?> GetByEnrollmentAsync(string enrollment);

        Task<bool> ExistsByEnrollmentAsync(string enrollment);
    }
}
