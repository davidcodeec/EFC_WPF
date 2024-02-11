using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public interface IAddressRepository
{
    Task<IEnumerable<AddressEntity>> GetAllAsync();
    Task<IEnumerable<AddressEntity>> GetAsync(Expression<Func<AddressEntity, bool>> predicate, int take);
    Task<AddressEntity> GetOneAsync(Expression<Func<AddressEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<AddressEntity, bool>> predicate);
    Task<AddressEntity?> CreateAsync(AddressEntity entity);
    Task<AddressEntity?> UpdateAsync(Expression<Func<AddressEntity, bool>> predicate, AddressEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<AddressEntity, bool>> predicate);
}