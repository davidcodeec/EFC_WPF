using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public interface ISkillService
{
    Task<SkillDto> CreateSkillAsync(string skillName);
    Task<IEnumerable<SkillDto>> GetSkillsAsync(Expression<Func<SkillEntity, bool>> predicate, int take = -1);
    Task<bool> DeleteSkillAsync(Expression<Func<SkillEntity, bool>> predicate);
    Task<IEnumerable<SkillDto>> GetAllSkillAsync();
    Task<SkillDto> GetOneSkillAsync(Expression<Func<SkillEntity, bool>> predicate);
    Task<SkillDto?> UpdateSkillAsync(UpdatedSkillDto updatedSkillDto);
}