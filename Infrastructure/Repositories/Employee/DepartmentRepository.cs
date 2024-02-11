using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class DepartmentRepository : GenericRepository<DepartmentEntity>, IDepartmentRepository
{
    private readonly ILogs _departmentLogs;

    public DepartmentRepository(EmployeeDbContext employeeDbContext, ILogs logs) : base(employeeDbContext, logs)
    {
        _departmentLogs = logs;
    }

    public override Task<DepartmentEntity?> CreateAsync(DepartmentEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<DepartmentEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<DepartmentEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<DepartmentEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.Departments
                .Include(i => i.Employees)
                .ToListAsync();

            return entities ?? Enumerable.Empty<DepartmentEntity>();
        }
        catch (Exception ex)
        {
            await _departmentLogs.LogToFileAsync($"Error in GetAllAsync: {ex}", "DepartmentRepository");
            return Enumerable.Empty<DepartmentEntity>();
        }
    }

    public async override Task<IEnumerable<DepartmentEntity>> GetAsync(Expression<Func<DepartmentEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.Departments
                .Include(i => i.Employees)
                .Where(predicate)
                .Take(take)
                .ToListAsync();

            return entities ?? Enumerable.Empty<DepartmentEntity>();
        }
        catch (Exception ex)
        {
            await _departmentLogs.LogToFileAsync($"Error in GetAsync: {ex}", "DepartmentRepository");
            return Enumerable.Empty<DepartmentEntity>();
        }
    }

    public async override Task<DepartmentEntity?> GetOneAsync(Expression<Func<DepartmentEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.Departments
                .Include(i => i.Employees)
                .FirstOrDefaultAsync(predicate);

            return entity;
        }
        catch (Exception ex)
        {
            await _departmentLogs.LogToFileAsync($"Error in GetOneAsync: {ex}", "DepartmentRepository");
            return null; // Return null in case of an exception
        }
    }

    public async override Task<DepartmentEntity?> UpdateAsync(Expression<Func<DepartmentEntity, bool>> predicate, DepartmentEntity updatedEntity)
    {
        try
        {

            // Find the existing entity in the database
            var entity = await _employeeDbContext.Departments.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
   

                // Create a new instance of DepartmentEntity and copy property values
                var newEntity = new DepartmentEntity
                {
                    DepartmentId = entity.DepartmentId,
                    DepartmentName = updatedEntity.DepartmentName // Copy only non-key property values
                };

                // Attach the new entity to the context and mark it as modified
                _employeeDbContext.Entry(entity).CurrentValues.SetValues(newEntity);

  
                await _employeeDbContext.SaveChangesAsync();

                return newEntity;
            }
            else
            {
  
                return null;
            }
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.ToString());

            await _logs.LogToFileAsync(ex.ToString(), $"{nameof(DepartmentRepository)} - UpdateAsync");
            throw;
        }
    }

}



