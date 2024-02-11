using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class PositionRepository(EmployeeDbContext employeeDbContext, ILogs logs) : GenericRepository<PositionEntity>(employeeDbContext, logs), IPositionRepository
{
    public override Task<PositionEntity?> CreateAsync(PositionEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<PositionEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<PositionEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<PositionEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.Positions
                .Include(i => i.Employees)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            // Log the exception with additional context
            await _logs.LogToFileAsync($"Exception occurred in GetAllAsync: {ex}", "PositionRepository");

            // Re-throw the exception to be handled by the caller
            throw;
        }
    }


    public async override Task<IEnumerable<PositionEntity>> GetAsync(Expression<Func<PositionEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.Positions
                .Include(i => i.Employees)
                .Where(predicate) // Apply the predicate to filter entities
                .Take(take)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "PositionRepository - GetAsync");
            throw;
        }
    }


    public async override Task<PositionEntity> GetOneAsync(Expression<Func<PositionEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.Positions
                .Include(i => i.Employees)
                .FirstOrDefaultAsync(predicate);

            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "PositionRepository - GetOneAsync"); }
        return null!;
    }

    public override Task<PositionEntity?> UpdateAsync(Expression<Func<PositionEntity, bool>> predicate, PositionEntity updatedEntity)
    {
        return base.UpdateAsync(predicate, updatedEntity);
    }
}
