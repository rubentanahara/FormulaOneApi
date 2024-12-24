// Achievements repository class that implements the IAchievementRepository interface. It is responsible for handling the CRUD operations for the Achievement entity.

using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.Data;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class AchievementRepository : GenericRepository<Achievement>, IAchievementsRepository
{
    public AchievementRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public async Task<Achievement?> GetDriverAchievementAsync(Guid driverId)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.DriverId == driverId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} GetDriverAchievementAsync method error", typeof(AchievementRepository));
            throw;
        }
    }

    public override async Task<IEnumerable<Achievement>> All()
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
              .AsNoTracking()
              .AsSplitQuery()
              .OrderBy(x => x.CreatedAt)
              .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} All method error", typeof(AchievementRepository));
            throw;
        }
    }

    public override async Task<bool> Delete(Guid id)
    {
        try
        {
            Achievement? entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return false;
            }

            entity.Status = 0;
            entity.UpdatedAt = DateTime.UtcNow;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} Delete method error", typeof(AchievementRepository));
            throw;
        }
    }

    public override async Task<bool> Update(Achievement achievement)
    {
        try
        {
            Achievement? entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == achievement.Id);

            if (entity is null)
            {
                return false;
            }

            entity.UpdatedAt = DateTime.UtcNow;
            entity.RaceWins = achievement.RaceWins;
            entity.PolePositions = achievement.PolePositions;
            entity.FastestLap = achievement.FastestLap;
            entity.WorldChampionships = achievement.WorldChampionships;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} Update method error", typeof(AchievementRepository));
            throw;
        }
    }
}
