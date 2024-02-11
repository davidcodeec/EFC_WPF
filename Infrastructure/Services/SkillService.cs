using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using System.Linq.Expressions;
namespace Infrastructure.Services;

public class SkillService(ISkillRepository skillRepository, ILogs logs) : ISkillService
{
    private readonly ISkillRepository _skillRepository = skillRepository;
    private readonly ILogs _logs = logs;


    public async Task<IEnumerable<SkillDto>> GetSkillsAsync(Expression<Func<SkillEntity, bool>> predicate, int take = -1)
    {
        try
        {
            var skillEntities = await _skillRepository.GetAsync(predicate, take);

            if (skillEntities != null)
            {
                return SkillEntity.Create(skillEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SkillService - GetPositionsAsync");
        }

        return Enumerable.Empty<SkillDto>();
    }


    public async Task<SkillDto> CreateSkillAsync(string skillName)
    {
        try
        {
            if (!await _skillRepository.ExistsAsync(x => x.SkillName == skillName))
            {
                var skillEntity = await _skillRepository.CreateAsync(new SkillDto { SkillName = skillName });
                return skillEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SkillService - CreateSkillAsync");
        }
        return null!;
    }

    public async Task<SkillDto> GetOneSkillAsync(Expression<Func<SkillEntity, bool>> predicate)
    {
        try
        {
            var skillEntity = await _skillRepository.GetOneAsync(predicate);

            if (skillEntity != null)
            {
                return skillEntity;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SkillService - GetOneSkillAsync");
        }
        return null!;
    }

    public async Task<IEnumerable<SkillDto>> GetAllSkillAsync()
    {
        try
        {
            var skillEntities = await _skillRepository.GetAllAsync();
            if (skillEntities != null)
            {
                return SkillEntity.Create(skillEntities);
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SkillService - GetAllDepartmentAsync");
        }
        return null!;
    }


    public async Task<SkillDto?> UpdateSkillAsync(UpdatedSkillDto updatedSkillDto)
    {
        try
        {
            
            var existingSkillEntity = await _skillRepository.GetOneAsync(x => x.SkillId == updatedSkillDto.Id);

            if (existingSkillEntity != null)
            {
               
                if (updatedSkillDto.SkillName != null)
                {
                    
                    existingSkillEntity.SkillName = updatedSkillDto.SkillName;

                   
                    var updatedSkillEntity = await _skillRepository.UpdateAsync(x => x.SkillId == updatedSkillDto.Id, existingSkillEntity);

                    if (updatedSkillDto != null)
                    {
                        
                        return updatedSkillEntity;
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
            await _logs.LogToFileAsync(ex.ToString(), "SkillService - UpdateSkillAsync");
        }

        
        return null;
    }

    public async Task<bool> DeleteSkillAsync(Expression<Func<SkillEntity, bool>> predicate)
    {
        try
        {
            return await _skillRepository.DeleteAsync(predicate);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "SkillService - DeleteSkillAsync");
        }
        return false;
    }
}
