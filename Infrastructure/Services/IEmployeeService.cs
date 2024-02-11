using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public interface IEmployeeService
{
    Task<EmployeeDto?> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
    Task<bool> DeleteEmployeeAsync(Expression<Func<EmployeeEntity, bool>> predicate);
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Expression<Func<EmployeeEntity, bool>> predicate, int take = -1);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeeAsync();
    Task<EmployeeDto?> GetOneEmployeeAsync(Expression<Func<EmployeeEntity, bool>> predicate);
    Task<EmployeeDto?> UpdateEmployeeAsync(Expression<Func<EmployeeEntity, bool>> predicate, UpdatedEmployeeDto updatedEmployeeDto);
}