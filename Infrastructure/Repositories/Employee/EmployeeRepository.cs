using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public class EmployeeRepository(EmployeeDbContext employeeDbContext, ILogs logs) : GenericRepository<EmployeeEntity>(employeeDbContext, logs), IEmployeeRepository
{
    public override Task<EmployeeEntity?> CreateAsync(EmployeeEntity entity)
    {
        return base.CreateAsync(entity);
    }

    public override Task<bool> DeleteAsync(Expression<Func<EmployeeEntity, bool>> predicate)
    {
        return base.DeleteAsync(predicate);
    }

    public override Task<bool> ExistsAsync(Expression<Func<EmployeeEntity, bool>> predicate)
    {
        return base.ExistsAsync(predicate);
    }

    public async override Task<IEnumerable<EmployeeEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.Employees
                .Include(i => i.Department)
                .Include(i => i.Position)
                .Include(i => i.Salary)
                .Include(i => i.Skill)
                .Include(i => i.EmployeePhoneNumbers).ThenInclude(i => i.PhoneNumber)
                .Include(i => i.EmployeeAddresses).ThenInclude(i => i.Address)
                .ToListAsync();

            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "EmployeeRepository - GetAllAsync"); }
        return null!;
    }

    public async override Task<IEnumerable<EmployeeEntity>> GetAsync(Expression<Func<EmployeeEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.Employees
                .Include(i => i.Department)
                .Include(i => i.Position)
                .Include(i => i.Salary)
                .Include(i => i.Skill)
                .Include(i => i.EmployeePhoneNumbers).ThenInclude(i => i.PhoneNumber)
                .Include(i => i.EmployeeAddresses).ThenInclude(i => i.Address)
                .Take(take).ToListAsync();

            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), "EmployeeRepository - GetAsync"); }
        return Enumerable.Empty<EmployeeEntity>();
    }

    public async override Task<EmployeeEntity?> GetOneAsync(Expression<Func<EmployeeEntity, bool>> predicate)
    {
        try
        {
            // Retrieve the employee entity with related entities included based on the predicate
            var entity = await _employeeDbContext.Employees
                .Include(i => i.Department)
                .Include(i => i.Position)
                .Include(i => i.Salary)
                .Include(i => i.Skill)
                .Include(i => i.EmployeePhoneNumbers)
                .Include(i => i.EmployeeAddresses).ThenInclude(i => i.Address)
                .FirstOrDefaultAsync(predicate);

            return entity; // Return null if not found
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow it
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeRepository - GetOneAsync");
            throw; // Rethrow the exception to let the caller handle it
        }
    }

    public async override Task<EmployeeEntity?> UpdateAsync(Expression<Func<EmployeeEntity, bool>> predicate, EmployeeEntity updatedEntity)
    {
        try
        {
            // Find the existing employee entity based on the predicate
            var existingEntity = await _employeeDbContext.Employees.FirstOrDefaultAsync(predicate);

            if (existingEntity != null)
            {
               

                // Update properties of the existing entity
                existingEntity.FirstName = updatedEntity.FirstName;
                existingEntity.LastName = updatedEntity.LastName;
                existingEntity.Email = updatedEntity.Email;
                existingEntity.BirthDate = updatedEntity.BirthDate;
                existingEntity.Gender = updatedEntity.Gender;
                existingEntity.DepartmentId = updatedEntity.DepartmentId;
                existingEntity.PositionId = updatedEntity.PositionId;
                existingEntity.SalaryId = updatedEntity.SalaryId;
                existingEntity.SkillId = updatedEntity.SkillId;

                // Save changes to the database
                await _employeeDbContext.SaveChangesAsync();
               

                // Return the updated entity
                return existingEntity;
            }
            else
            {
               
                return null; // Return null if the entity to update is not found
            }
        }
        catch (Exception ex)
        {
            
            throw; // Rethrow the exception to let the caller handle it
        }
    }






}
