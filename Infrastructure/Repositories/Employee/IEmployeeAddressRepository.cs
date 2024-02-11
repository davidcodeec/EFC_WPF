using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface IEmployeeAddressRepository
{
    Task<IEnumerable<EmployeeAddressEntity>> GetAllAsync();
    Task<IEnumerable<EmployeeAddressEntity>> GetAsync(Expression<Func<EmployeeAddressEntity, bool>> predicate, int take);
    Task<EmployeeAddressEntity> GetOneAsync(Expression<Func<EmployeeAddressEntity, bool>> predicate);
}