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

        public async Task<IEnumerable<TimekeepingModel>> GetAllPointsAsync(int pageNumber, int pageSize, int? month = null, int? year = null)
        {
            var query = _appDbContext.Set<TimekeepingModel>().AsQueryable();

            if (month.HasValue && year.HasValue)
            {
                query = query.Where(t => t.ClockIn.HasValue && t.ClockIn.Value.Month == month.Value && t.ClockIn.Value.Year == year.Value);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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