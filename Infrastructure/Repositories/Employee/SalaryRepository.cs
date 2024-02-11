using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class SalaryRepository(EmployeeDbContext employeeDbContext, ILogs logs) : GenericRepository<SalaryEntity>(employeeDbContext, logs), ISalaryRepository
{
    public override Task<SalaryEntity?> CreateAsync(SalaryEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<SalaryEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<SalaryEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<SalaryEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.Salaries
                .Include(i => i.Employees)
                .ToListAsync();

            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "SalaryRepository - GetAllAsync"); }
        return null!;
    }


    public async override Task<IEnumerable<SalaryEntity>> GetAsync(Expression<Func<SalaryEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.Salaries
                .Where(predicate)
                .Take(take)
                .ToListAsync();

            if (entities.Count != 0)
            {
                return entities;
            }
            else
            {
                return Enumerable.Empty<SalaryEntity>(); // Return an empty enumerable if no entities are found
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SalaryRepository - GetAsync");
            throw; // Rethrow the exception after logging
        }
    }


    public async override Task<SalaryEntity> GetOneAsync(Expression<Func<SalaryEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.Salaries
                .Include(i => i.Employees)
                .FirstOrDefaultAsync(predicate);

            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "SalaryRepository - GetOneAsync"); }
        return null!;
    }

    public override Task<SalaryEntity?> UpdateAsync(Expression<Func<SalaryEntity, bool>> predicate, SalaryEntity updatedEntity)
    {
        return base.UpdateAsync(predicate, updatedEntity);
    }
}

