using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeEntity>> GetAllAsync();
    Task<IEnumerable<EmployeeEntity>> GetAsync(Expression<Func<EmployeeEntity, bool>> predicate, int take);
    Task<EmployeeEntity> GetOneAsync(Expression<Func<EmployeeEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<EmployeeEntity, bool>> predicate);
    Task<EmployeeEntity> CreateAsync(EmployeeEntity entity);
    Task<EmployeeEntity?> UpdateAsync(Expression<Func<EmployeeEntity, bool>> predicate, EmployeeEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<EmployeeEntity, bool>> predicate);
}