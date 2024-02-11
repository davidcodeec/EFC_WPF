using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class DepartmentService(IDepartmentRepository departmentRepository, ILogs logs) : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly ILogs _logs = logs;

    public async Task<DepartmentDto> CreateDepartmentAsync(string departmentName) 
    {
        try
        {
            if (!await _departmentRepository.ExistsAsync(x => x.DepartmentName == departmentName))
            {
                var departmentEntity = await _departmentRepository.CreateAsync(new DepartmentDto { DepartmentName = departmentName });
                return departmentEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - CreateDepartmentAsync");
        }
        return null; 
    }

    public async Task<DepartmentDto> GetOneDepartmentAsync(Expression<Func<DepartmentEntity, bool>> predicate)
    {
        try
        {
            var departmentEntity = await _departmentRepository.GetOneAsync(predicate);

            if (departmentEntity != null)
            {
                return departmentEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - GetOneDepartmentAsync");
        }
        return null!;
    }


    public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(Expression<Func<DepartmentEntity, bool>> predicate, int take = -1)
    {
        try
        {
            var departmentEntities = await _departmentRepository.GetAsync(predicate, take);

            if (departmentEntities != null)
            {
                return DepartmentEntity.Create(departmentEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - GetDepartmentsAsync");
        }

        return Enumerable.Empty<DepartmentDto>();
    }



    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentAsync()
    {
        try
        {
            var departmentEntities = await _departmentRepository.GetAllAsync();
            if (departmentEntities != null)
            {
                return DepartmentEntity.Create(departmentEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - GetAllDepartmentAsync");
        }
        return null!;
    }

    public async Task<DepartmentDto?> UpdateDepartmentAsync(UpdatedDepartmentDto updatedDepartmentDto)
    {
        try
        {
            // Retrieve the existing department entity
            var existingDepartmentEntity = await _departmentRepository.GetOneAsync(x => x.DepartmentId == updatedDepartmentDto.Id);

            if (existingDepartmentEntity != null)
            {
                // Ensure that the DepartmentName from the DTO is not null before assigning
                if (updatedDepartmentDto.DepartmentName != null)
                {
                    // Update the properties of the existing entity
                    existingDepartmentEntity.DepartmentName = updatedDepartmentDto.DepartmentName;

                    // Use the existing entity for the update operation
                    var updatedDepartmentEntity = await _departmentRepository.UpdateAsync(x => x.DepartmentId == updatedDepartmentDto.Id, existingDepartmentEntity);

                    if (updatedDepartmentEntity != null)
                    {
                        // Convert the updated entity to DTO
                        return updatedDepartmentEntity;
                    }
                }
                else
                {
                    await _logs.LogWarningAsync("DepartmentName from the DTO is null during department update.");
                }
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - UpdateDepartmentAsync");
        }

        // Return null if the update operation fails, DepartmentName is null, or an exception occurs
        return null;
    }


    public async Task<bool> DeleteDepartmentAsync(Expression<Func<DepartmentEntity, bool>> predicate)
    {
        try
        {
            bool deletionResult = await _departmentRepository.DeleteAsync(predicate);
            return deletionResult;
        }
        catch (Exception ex)
        {
            // Log the exception
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - DeleteDepartmentAsync");
            // Return false indicating deletion failure
            return false;
        }
    }

}
