using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public interface IPositionService
{
    Task<PositionDto> CreatePositionAsync(string positionName);
    Task<IEnumerable<PositionDto>> GetPositionsAsync(Expression<Func<PositionEntity, bool>> predicate, int take = -1);
    Task<bool> DeletePositionAsync(Expression<Func<PositionEntity, bool>> predicate);
    Task<IEnumerable<PositionDto>> GetAllPositionAsync();
    Task<PositionDto> GetOnePositionAsync(Expression<Func<PositionEntity, bool>> predicate);
    Task<PositionDto?> UpdatePostionAsync(UpdatedPositionDto updatedPositionDto);
}