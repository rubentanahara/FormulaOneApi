// Unit of Work pattern implementation

using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.Data;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{

    private readonly AppDbContext _context;


    public IDriverRepository DriverRepository { get; }

    public IAchievementsRepository AchievementsRepository { get; }

    public UnitOfWork(AppDbContext context, ILoggerFactory logger)
    {
        _context = context;

        DriverRepository = new DriverRepository(_context, logger.CreateLogger<DriverRepository>());
        AchievementsRepository = new AchievementRepository(_context, logger.CreateLogger<AchievementRepository>());
    }

    public async Task<bool> CompleteAsync()
    {
        var results = await _context.SaveChangesAsync();
        return results > 0;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
