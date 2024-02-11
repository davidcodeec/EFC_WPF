using Infrastructure.Dtos;
using Infrastructure.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public interface IAddressService
    {
        Task<AddressDto> CreateAddressAsync(string streetName, string streetNumber, string postalCode, string city);
        Task<bool> DeleteAddressAsync(Expression<Func<AddressEntity, bool>> predicate);
        Task<IEnumerable<AddressDto>> GetAddressesAsync(Expression<Func<AddressEntity, bool>> predicate, int take = -1);
        Task<IEnumerable<AddressDto>> GetAllAddressesAsync();
        Task<AddressDto> GetOneAddressAsync(Expression<Func<AddressEntity, bool>> predicate);
        Task<AddressEntity?> UpdateAddressAsync(int addressId, UpdatedAddressDto updatedAddressDto);
    }
}