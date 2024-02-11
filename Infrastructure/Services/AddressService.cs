using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class AddressService(IAddressRepository addressRepository, ILogs logs) : IAddressService
{
    private readonly ILogs _logs = logs;
    private readonly IAddressRepository _addressRepository = addressRepository;

    public async Task<IEnumerable<AddressDto>> GetAddressesAsync(Expression<Func<AddressEntity, bool>> predicate, int take = -1)
    {
        try
        {
            var addressEntities = await _addressRepository.GetAsync(predicate, take);
            return AddressEntity.Create(addressEntities);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "AddressService - GetAddressesAsync");
            return Enumerable.Empty<AddressDto>();
        }
    }

    public async Task<AddressDto> CreateAddressAsync(string streetName, string streetNumber, string postalCode, string city)
    {
        try
        {
            var addressEntity = await _addressRepository.CreateAsync(new AddressEntity
            {
                StreetName = streetName,
                StreetNumber = streetNumber,
                PostalCode = postalCode,
                City = city
            });
            return addressEntity;
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "AddressService - CreateAddressAsync");
            return null;
        }
    }

    public async Task<AddressDto> GetOneAddressAsync(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            var addressEntity = await _addressRepository.GetOneAsync(predicate);
            if (addressEntity != null)
            {
                var addressDto = new AddressDto
                {
                    AddressId = addressEntity.AddressId,
                    StreetName = addressEntity.StreetName,
                    StreetNumber = addressEntity.StreetNumber,
                    PostalCode = addressEntity.PostalCode,
                    City = addressEntity.City
                    // Set other properties as needed
                };
                return addressDto;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "AddressService - GetAddressForPersonAsync");
            throw;
        }
    }


    public async Task<IEnumerable<AddressDto>> GetAllAddressesAsync()
    {
        try
        {
            var addressEntities = await _addressRepository.GetAllAsync();
            return AddressEntity.Create(addressEntities);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "AddressService - GetAllAddressesAsync");
            throw; 
        }
    }

    public async Task<AddressEntity?> UpdateAddressAsync(int addressId, UpdatedAddressDto updatedAddressDto)
    {
        try
        {
            var existingAddressEntity = await _addressRepository.GetOneAsync(x => x.AddressId == addressId);
            if (existingAddressEntity != null)
            {
                existingAddressEntity.StreetName = updatedAddressDto.StreetName ?? existingAddressEntity.StreetName;
                existingAddressEntity.StreetNumber = updatedAddressDto.StreetNumber ?? existingAddressEntity.StreetNumber;
                existingAddressEntity.PostalCode = updatedAddressDto.PostalCode ?? existingAddressEntity.PostalCode;
                existingAddressEntity.City = updatedAddressDto.City ?? existingAddressEntity.City;

                var updatedAddressEntity = await _addressRepository.UpdateAsync(x => x.AddressId == addressId, existingAddressEntity);
                return updatedAddressEntity;
            }
            else
            {
                // Address with given ID not found
                return null;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "AddressService - UpdateAddressAsync");
            return null;
        }
    }



    public async Task<bool> DeleteAddressAsync(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            return await _addressRepository.DeleteAsync(predicate);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "AddressService - DeleteAddressAsync");
            return false;
        }
    }
}
