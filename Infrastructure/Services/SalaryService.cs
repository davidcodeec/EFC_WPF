using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class SalaryService(ISalaryRepository salaryRepository, ILogs logs) : ISalaryService
{
    private readonly ISalaryRepository _salaryRepository = salaryRepository;
    private readonly ILogs _logs = logs;

    public async Task<IEnumerable<SalaryDto>> GetSalariesAsync(Expression<Func<SalaryEntity, bool>> predicate, int take = -1)
    {
        try
        {
            var salaryEntities = await _salaryRepository.GetAsync(predicate, take);

            if (salaryEntities != null)
            {
                return SalaryEntity.Create(salaryEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - GetDepartmentsAsync");
        }

        return Enumerable.Empty<SalaryDto>();
    }

    public async Task<SalaryDto> CreateSalaryAsync(decimal amount, DateTime startDate, DateTime endDate)
    {
        try
        {
            var salaryEntity = new SalaryEntity { Amount = amount, StartDate = startDate, EndDate = endDate };

            if (!await _salaryRepository.ExistsAsync(x => x.Amount == amount && x.StartDate == startDate && x.EndDate == endDate))
            {
                var createdSalaryEntity = await _salaryRepository.CreateAsync(salaryEntity);
                return createdSalaryEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SalaryService - CreateSalaryAsync");
        }
        return null!;
    }

    public async Task<SalaryDto> GetOneSalaryAsync(Expression<Func<SalaryEntity, bool>> predicate)
    {
        try
        {
            var salaryEntity = await _salaryRepository.GetOneAsync(predicate);

            if (salaryEntity != null)
            {
                return salaryEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SalaryService - GetOneSalaryAsync");
        }
        return null!;
    }

    public async Task<IEnumerable<SalaryDto>> GetAllSalaryAsync()
    {
        try
        {
            var salaryEntities = await _salaryRepository.GetAllAsync();
            if (salaryEntities != null)
            {
                return SalaryEntity.Create(salaryEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SalaryService - GetAllPositionAsync");
        }
        return null!;
    }


    public async Task<SalaryDto?> UpdateSalaryAsync(UpdatedSalaryDto updatedSalaryDto)
    {
        try
        {
           
            var existingSalaryEntity = await _salaryRepository.GetOneAsync(x => x.SalaryId == updatedSalaryDto.SalaryId);

            if (existingSalaryEntity != null)
            {
                
                if (updatedSalaryDto.Amount != null)
                {
                    
                    existingSalaryEntity.Amount = updatedSalaryDto.Amount ?? 0;
                    existingSalaryEntity.StartDate = updatedSalaryDto.StartDate;
                    existingSalaryEntity.EndDate = updatedSalaryDto.EndDate;

                    
                    var updatedSalaryEntity = await _salaryRepository.UpdateAsync(x => x.SalaryId == updatedSalaryDto.SalaryId, existingSalaryEntity);

                    if (updatedSalaryEntity != null)
                    {
                        
                        return updatedSalaryEntity; 
                    }
                }
                else
                {
                    await _logs.LogWarningAsync("Amount from the DTO is null during salary update.");
                }
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SalaryService - UpdateSalaryAsync");
        }

        
        return null;
    }


    public async Task<bool> DeleteSalaryAsync(Expression<Func<SalaryEntity, bool>> predicate)
    {
        try
        {
            return await _salaryRepository.DeleteAsync(predicate);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SalaryService - DeleteSalaryAsync");
        }
        return false;
    }

}
