using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public interface ISalaryService
{
    Task<SalaryDto> CreateSalaryAsync(decimal amount, DateTime startDate, DateTime endDate);
    Task<IEnumerable<SalaryDto>> GetSalariesAsync(Expression<Func<SalaryEntity, bool>> predicate, int take = -1);
    Task<bool> DeleteSalaryAsync(Expression<Func<SalaryEntity, bool>> predicate);
    Task<IEnumerable<SalaryDto>> GetAllSalaryAsync();
    Task<SalaryDto> GetOneSalaryAsync(Expression<Func<SalaryEntity, bool>> predicate);
    Task<SalaryDto?> UpdateSalaryAsync(UpdatedSalaryDto updatedSalaryDto);
}