using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class EmployeePhoneNumberRepository(EmployeeDbContext employeeDbContext, ILogs logs) : GenericRepository<EmployeePhoneNumberEntity>(employeeDbContext, logs), IEmployeePhoneNumberRepository
{
    public override Task<EmployeePhoneNumberEntity?> CreateAsync(EmployeePhoneNumberEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<EmployeePhoneNumberEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<EmployeePhoneNumberEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<EmployeePhoneNumberEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.EmployeePhoneNumbers
                .Include(i => i.Employee)
                .ToListAsync();

            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "EmployeePhoneNumberRepository - GetAllAsync"); }
        return null!;
    }

    public async override Task<IEnumerable<EmployeePhoneNumberEntity>> GetAsync(Expression<Func<EmployeePhoneNumberEntity, bool>> predicate, int take)
    {
        try
        {

            var entities = await _employeeDbContext.EmployeePhoneNumbers
                .Include(i => i.Employee)
                .Where(predicate) // Apply the predicate to filter the entities
                .Take(take)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeePhoneNumberRepository - GetAsync");
            throw;
        }
    }




    public async override Task<EmployeePhoneNumberEntity> GetOneAsync(Expression<Func<EmployeePhoneNumberEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.EmployeePhoneNumbers
                .Include(i => i.Employee)
                .FirstOrDefaultAsync(predicate);

            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "EmployeePhoneNumberRepository - GetOneAsync"); }
        return null!;
    }

    public async override Task<EmployeePhoneNumberEntity?> UpdateAsync(Expression<Func<EmployeePhoneNumberEntity, bool>> predicate, EmployeePhoneNumberEntity updatedEntity)
    {
        try
        {
            // Find the existing entity to update
            var existingEntity = await _employeeDbContext.EmployeePhoneNumbers.FirstOrDefaultAsync(predicate);
            if (existingEntity != null)
            {
                // Update the AddressId property of the existing entity
                existingEntity.Id = updatedEntity.Id;

                // Save changes to the database
                await _employeeDbContext.SaveChangesAsync();

                return existingEntity;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeePhoneNumberRepository - UpdateAsync");
            throw;
        }
    }
}

