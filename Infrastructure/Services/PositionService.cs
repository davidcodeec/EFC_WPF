using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class PositionService(IPositionRepository positionRepository, ILogs logs) : IPositionService
{
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly ILogs _logs = logs;


    public async Task<IEnumerable<PositionDto>> GetPositionsAsync(Expression<Func<PositionEntity, bool>> predicate, int take = -1)
    {
        try
        {
            var positionEntities = await _positionRepository.GetAsync(predicate, take);

            if (positionEntities != null)
            {
                return PositionEntity.Create(positionEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "DepartmentService - GetDepartmentsAsync");
        }

        return Enumerable.Empty<PositionDto>();
    }

    public async Task<PositionDto> CreatePositionAsync(string positionName)
    {
        try
        {
            if (!await _positionRepository.ExistsAsync(x => x.PositionName == positionName))
            {
                var positionEntity = await _positionRepository.CreateAsync(new PositionDto { PositionName = positionName });
                return positionEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "PositionService - CreateSkillAsync");
        }
        return null!;
    }

    public async Task<PositionDto> GetOnePositionAsync(Expression<Func<PositionEntity, bool>> predicate)
    {
        try
        {
            var positionEntity = await _positionRepository.GetOneAsync(predicate);

            if (positionEntity != null)
            {
                return positionEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "PositionService - GetOneSkillAsync");
        }
        return null!;
    }

    public async Task<IEnumerable<PositionDto>> GetAllPositionAsync()
    {
        try
        {
            var positionEntities = await _positionRepository.GetAllAsync();
            if (positionEntities != null)
            {
                return PositionEntity.Create(positionEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "PositionService - GetAllPositionAsync");
        }
        return null!;
    }


    public async Task<PositionDto?> UpdatePostionAsync(UpdatedPositionDto updatedPositionDto)
    {
        try
        {
            // Retrieve the existing department entity
            var existingPositionEntity = await _positionRepository.GetOneAsync(x => x.PositionId == updatedPositionDto.Id);

            if (existingPositionEntity != null)
            {
                // Ensure that the PositionName from the DTO is not null before assigning
                if (updatedPositionDto.PositionName != null)
                {
                    // Update the properties of the existing entity
                    existingPositionEntity.PositionName = updatedPositionDto.PositionName;

                    // Use the existing entity for the update operation
                    var updatedPositionEntity = await _positionRepository.UpdateAsync(x => x.PositionId == updatedPositionDto.Id, existingPositionEntity);

                    if (updatedPositionDto != null)
                    {
                        // Convert the updated entity to DTO
                        return updatedPositionEntity;
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
            await _logs.LogToFileAsync(ex.ToString(), "PositionService - UpdatePostionAsync");
        }

        
        return null;
    }

    public async Task<bool> DeletePositionAsync(Expression<Func<PositionEntity, bool>> predicate)
    {
        try
        {
            return await _positionRepository.DeleteAsync(predicate);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "PositionService - DeletePositionAsync");
        }
        return false;
    }

}
