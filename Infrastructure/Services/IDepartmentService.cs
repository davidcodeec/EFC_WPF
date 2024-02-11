using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Linq.Expressions;


namespace Infrastructure.Services;

public interface IDepartmentService
{
    Task<DepartmentDto> CreateDepartmentAsync(string departmentName);
    Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(Expression<Func<DepartmentEntity, bool>> predicate, int take = -1);
    Task<bool> DeleteDepartmentAsync(Expression<Func<DepartmentEntity, bool>> predicate);
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentAsync();
    Task<DepartmentDto> GetOneDepartmentAsync(Expression<Func<DepartmentEntity, bool>> predicate);
    Task<DepartmentDto?> UpdateDepartmentAsync(UpdatedDepartmentDto updatedDepartmentDto);

}
