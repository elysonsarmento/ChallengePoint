using ChallengePoint.Domain.Models;

namespace ChallengePoint.Domain.Interface
{
    public interface IPointRepository
    {
        Task AddPointAsync(TimekeepingModel timekeeping);

        Task<IEnumerable<TimekeepingModel>> GetAllPointsAsync(int pageNumber, int pageSize, int? month = null, int? year = null);

        Task<TimekeepingModel?> GetClockInByDateAsync(int collaboratorId, DateTime date);

        Task<TimekeepingModel?> GetLastClockInAsync(int collaboratorId);

        Task<TimekeepingModel?> GetClockOutByDateAsync(int collaboratorId, DateTime date);

        Task<TimekeepingModel?> GetClockInForCollaboratorAndDayAsync(int collaboratorId, DateTime date);

        Task UpdatePointAsync(TimekeepingModel timekeeping);
    }
}