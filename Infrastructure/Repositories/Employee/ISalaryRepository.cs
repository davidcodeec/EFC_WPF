using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface ISalaryRepository
{
    Task<IEnumerable<SalaryEntity>> GetAllAsync();
    Task<IEnumerable<SalaryEntity>> GetAsync(Expression<Func<SalaryEntity, bool>> predicate, int take);
    Task<SalaryEntity> GetOneAsync(Expression<Func<SalaryEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<SalaryEntity, bool>> predicate);
    Task<SalaryEntity> CreateAsync(SalaryEntity entity);
    Task<SalaryEntity> UpdateAsync(Expression<Func<SalaryEntity, bool>> predicate, SalaryEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<SalaryEntity, bool>> predicate);
}