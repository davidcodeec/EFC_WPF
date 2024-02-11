using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class SkillRepository(EmployeeDbContext employeeDbContext, ILogs logs) : GenericRepository<SkillEntity>(employeeDbContext, logs), ISkillRepository
{
    public override Task<SkillEntity?> CreateAsync(SkillEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<SkillEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<SkillEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<SkillEntity>> GetAllAsync()
    {
        try
        {

            var entities = await _employeeDbContext.Skills
                .Include(i => i.Employees)
                .ToListAsync();


            if (entities.Count != 0)
            {
 
                return entities;
            }
        }
        catch (Exception ex)
        {
            // Log the exception to a file
            await _logs.LogToFileAsync(ex.ToString(), "SkillRepository - GetAllAsync");
        }

        return Enumerable.Empty<SkillEntity>();
    }



    public async override Task<IEnumerable<SkillEntity>> GetAsync(Expression<Func<SkillEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.Skills
                .Include(i => i.Employees)
                .Where(predicate) // Apply the provided predicate
                .Take(take)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SkillRepository - GetAsync");
            // This handle exceptions properly here, depending on your application's requirements???
            throw; // Rethrow the exception to propagate it
        }
    }


    public async override Task<SkillEntity> GetOneAsync(Expression<Func<SkillEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.Skills
                .Include(i => i.Employees)
                .FirstOrDefaultAsync(predicate);

            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "SkillRepository - GetOneAsync"); }
        return null!;
    }

    public override Task<SkillEntity?> UpdateAsync(Expression<Func<SkillEntity, bool>> predicate, SkillEntity updatedEntity)
    {
        return base.UpdateAsync(predicate, updatedEntity);
    }
}
