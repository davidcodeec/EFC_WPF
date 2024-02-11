using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface IPositionRepository
{
    Task<IEnumerable<PositionEntity>> GetAllAsync();
    Task<IEnumerable<PositionEntity>> GetAsync(Expression<Func<PositionEntity, bool>> predicate, int take);
    Task<PositionEntity> GetOneAsync(Expression<Func<PositionEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<PositionEntity, bool>> predicate);
    Task<PositionEntity> CreateAsync(PositionEntity entity);
    Task<PositionEntity> UpdateAsync(Expression<Func<PositionEntity, bool>> predicate, PositionEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<PositionEntity, bool>> predicate);
}