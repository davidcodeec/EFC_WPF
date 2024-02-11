using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class EmployeeAddressRepository(EmployeeDbContext employeeDbContext, ILogs logs) : GenericRepository<EmployeeAddressEntity>(employeeDbContext, logs), IEmployeeAddressRepository
{
    public override Task<EmployeeAddressEntity?> CreateAsync(EmployeeAddressEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<EmployeeAddressEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<EmployeeAddressEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<EmployeeAddressEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.EmployeeAddresses
                .Include(i => i.Employee)
                .Include(i => i.Address)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            // Log the exception details
            await _logs.LogToFileAsync($"Error occurred in GetAllAsync: {ex.Message}", "EmployeeAddressRepository - GetAllAsync");

            // Rethrow the exception after logging
            throw;
        }
    }


    public async override Task<IEnumerable<EmployeeAddressEntity>> GetAsync(Expression<Func<EmployeeAddressEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.EmployeeAddresses
                .Where(predicate)
                .Take(take)
                .ToListAsync();

            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeAddressRepository - GetAsync");
            throw;
        }

        return Enumerable.Empty<EmployeeAddressEntity>(); 
    }


    public async override Task<EmployeeAddressEntity> GetOneAsync(Expression<Func<EmployeeAddressEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.EmployeeAddresses
                .Include(i => i.Employee)
                .Include(i => i.Address)
                .FirstOrDefaultAsync(predicate);

            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "EmployeeAddressRepository - GetOneAsync"); }
        return null!;
    }



    public async override Task<EmployeeAddressEntity?> UpdateAsync(Expression<Func<EmployeeAddressEntity, bool>> predicate, EmployeeAddressEntity updatedEntity)
    {
        try
        {
            // Find the existing entity to update
            var existingEntity = await _employeeDbContext.EmployeeAddresses.FirstOrDefaultAsync(predicate);
            if (existingEntity != null)
            {
                // Update the AddressId property of the existing entity
                existingEntity.AddressId = updatedEntity.AddressId;

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
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeAddressRepository - UpdateAsync");
            throw;
        }
    }


}

