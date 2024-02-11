using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class AddressRepository(EmployeeDbContext employeeDbContext, ILogs logs) : GenericRepository<AddressEntity>(employeeDbContext, logs), IAddressRepository
{
    public override Task<AddressEntity?> CreateAsync(AddressEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<AddressEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<AddressEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<AddressEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.Addresses
                .Include(a => a.EmployeeAddresses)
                .ThenInclude(ea => ea.Employee)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), $"{nameof(AddressRepository)} - {nameof(GetAllAsync)}");
            throw;
        }
    }

    public async override Task<IEnumerable<AddressEntity>> GetAsync(Expression<Func<AddressEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.Addresses
                .Include(a => a.EmployeeAddresses)
                .ThenInclude(ea => ea.Employee)
                .Where(predicate)
                .Take(take)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), $"{nameof(AddressRepository)} - {nameof(GetAsync)}");
            throw; 
        }
    }

    public async override Task<AddressEntity?> GetOneAsync(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.Addresses
                .Include(a => a.EmployeeAddresses)
                .ThenInclude(ea => ea.Employee)
                .FirstOrDefaultAsync(predicate);

            return entity;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), $"{nameof(AddressRepository)} - {nameof(GetOneAsync)}");
            throw;
        }
    }

    public async override Task<AddressEntity?> UpdateAsync(Expression<Func<AddressEntity, bool>> predicate, AddressEntity updatedEntity)
    {
        try
        {
            var existingAddress = await _employeeDbContext.Addresses.FirstOrDefaultAsync(predicate);
            if (existingAddress != null)
            {
                existingAddress.StreetName = updatedEntity.StreetName;
                existingAddress.StreetNumber = updatedEntity.StreetNumber;
                existingAddress.PostalCode = updatedEntity.PostalCode;
                existingAddress.City = updatedEntity.City;

                await _employeeDbContext.SaveChangesAsync();
            }

            return existingAddress;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), $"{nameof(AddressRepository)} - {nameof(UpdateAsync)}");
            throw; 
        }
    }

}
