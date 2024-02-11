using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface IDepartmentRepository
{
    Task<IEnumerable<DepartmentEntity>> GetAllAsync();
    Task<IEnumerable<DepartmentEntity>> GetAsync(Expression<Func<DepartmentEntity, bool>> predicate, int take);
    Task<DepartmentEntity?> GetOneAsync(Expression<Func<DepartmentEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<DepartmentEntity, bool>> predicate);
    Task<DepartmentEntity?> CreateAsync(DepartmentEntity entity);
    Task<DepartmentEntity?> UpdateAsync(Expression<Func<DepartmentEntity, bool>> predicate, DepartmentEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<DepartmentEntity, bool>> predicate);
}
