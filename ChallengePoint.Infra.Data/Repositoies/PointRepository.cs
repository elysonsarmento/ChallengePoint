using ChallengePoint.Domain.Interface;
using ChallengePoint.Domain.Models;
using ChallengePoint.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ChallengePoint.Infrastructure.Repositories
{
    public class PointRepository(AppDbContext appDbContext) : IPointRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task AddPointAsync(TimekeepingModel timekeeping)
        {
            await _appDbContext.Set<TimekeepingModel>().AddAsync(timekeeping);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TimekeepingModel>> GetAllPointsAsync()
        {
            return await _appDbContext.Set<TimekeepingModel>().ToListAsync();
        }

        public async Task<TimekeepingModel?> GetClockInByDateAsync(int collaboratorId, DateTime date)
        {
            return await _appDbContext.Set<TimekeepingModel>()
                .Where(t => t.CollaboratorId == collaboratorId && t.ClockIn.HasValue && t.ClockIn.Value.Date == date.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<TimekeepingModel?> GetLastClockInAsync(int collaboratorId)
        {
            return await _appDbContext.Set<TimekeepingModel>()
                .Where(t => t.CollaboratorId == collaboratorId && t.ClockIn.HasValue)
                .OrderByDescending(t => t.ClockIn)
                .FirstOrDefaultAsync();
        }

        public async Task<TimekeepingModel?> GetClockOutByDateAsync(int collaboratorId, DateTime date)
        {
            return await _appDbContext.Set<TimekeepingModel>()
                .Where(t => t.CollaboratorId == collaboratorId && t.ClockOut.HasValue && t.ClockOut.Value.Date == date.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<TimekeepingModel?> GetClockInForCollaboratorAndDayAsync(int collaboratorId, DateTime date)
        {
            return await _appDbContext.Set<TimekeepingModel>()
                .Where(t => t.CollaboratorId == collaboratorId && t.ClockIn.HasValue && t.ClockIn.Value.Date == date.Date)
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePointAsync(TimekeepingModel timekeeping)
        {
            _appDbContext.Entry(timekeeping).State = EntityState.Modified; 
            await _appDbContext.SaveChangesAsync();
        }
    }
}