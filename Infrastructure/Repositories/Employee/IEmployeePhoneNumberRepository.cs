using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface IEmployeePhoneNumberRepository
{
    Task<IEnumerable<EmployeePhoneNumberEntity>> GetAllAsync();
    Task<IEnumerable<EmployeePhoneNumberEntity>> GetAsync(Expression<Func<EmployeePhoneNumberEntity, bool>> predicate, int take);
    Task<EmployeePhoneNumberEntity> GetOneAsync(Expression<Func<EmployeePhoneNumberEntity, bool>> predicate);
}