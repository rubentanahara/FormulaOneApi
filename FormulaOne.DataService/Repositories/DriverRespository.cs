// Driver Repository is a class that implements the IDriverRepository interface. It is responsible for handling the CRUD operations for the Driver entity.

using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.Data;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class DriverRepository : GenericRepository<Driver>, IDriverRepository
{
    public DriverRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public override async Task<IEnumerable<Driver>> All()
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
            _logger.LogError(ex, "{Repo} All method error", typeof(DriverRepository));
            throw;
        }
    }

    public override async Task<bool> Delete(Guid id)
    {
        try
        {
            _logger.LogInformation("Delete method called");
            Driver? entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

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
            _logger.LogError(ex, "{Repo} Delete method error", typeof(DriverRepository));
            throw;
        }
    }

    public override async Task<bool> Update(Driver driver)
    {
        try
        {
            _logger.LogInformation("Update method called");
            Driver? entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == driver.Id);

            if (entity is null)
            {
                return false;
            }

            entity.FirstName = entity.FirstName;
            entity.LastName = entity.LastName;
            entity.DateOfBirth = entity.DateOfBirth;
            entity.DriverNumber = entity.DriverNumber;
            entity.UpdatedAt = DateTime.UtcNow;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} Update method error", typeof(DriverRepository));
            throw;
        }
    }
}
