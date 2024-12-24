// GenericRepository.cs is a generic repository class that implements the IGenericRepository interface.

using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public readonly ILogger _logger;
    protected AppDbContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepository(
        AppDbContext context,
        ILogger logger
        )
    {
        _context = context;
        _logger = logger;

        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> Update(T entity)
    {
        await Task.CompletedTask;
        return false;
    }

    public virtual async Task<bool> Delete(Guid id)
    {
        await Task.CompletedTask;
        return false;
    }
}
