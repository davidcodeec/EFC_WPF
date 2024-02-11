using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface ISkillRepository
{
    Task<IEnumerable<SkillEntity>> GetAllAsync();
    Task<IEnumerable<SkillEntity>> GetAsync(Expression<Func<SkillEntity, bool>> predicate, int take);
    Task<SkillEntity> GetOneAsync(Expression<Func<SkillEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<SkillEntity, bool>> predicate);
    Task<SkillEntity> CreateAsync(SkillEntity entity);
    Task<SkillEntity> UpdateAsync(Expression<Func<SkillEntity, bool>> predicate, SkillEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<SkillEntity, bool>> predicate);
}