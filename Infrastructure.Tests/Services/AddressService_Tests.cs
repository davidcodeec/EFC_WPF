using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Services;
using Infrastructure.Utils;
using Moq;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Tests.Services;

public class AddressService_Tests
{
    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        var mockRepository = new Mock<IAddressRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<AddressEntity>()))
                      .ThrowsAsync(new Exception("Simulated exception"));

        var addressRepository = mockRepository.Object;

        IAddressService addressService = new AddressService(addressRepository, mockLogs.Object);

        // Act
        var result = await addressService.CreateAddressAsync(addressEntity.StreetName, addressEntity.StreetNumber, addressEntity.PostalCode, addressEntity.City);

        // Assert
        Assert.Null(result);

        
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_AddressDtos()
    {
        // Arrange
        var addressEntity = new List<AddressEntity>
            {
                new AddressEntity
                {
                    City = "Stockholm",
                    PostalCode = "11234",
                    StreetName = "Sveavägen",
                    StreetNumber = "45A",
                },
                
            };

        var mockRepository = new Mock<IAddressRepository>();

        
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(addressEntity);

        var addressRepository = mockRepository.Object;

        IAddressService addressService = new AddressService(addressRepository, mockLogs.Object);

        // Act
        var result = await addressService.GetAllAddressesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.All(dto => dto is AddressDto));
        Assert.Equal(addressEntity.Count, result.Count());
    }



    [Fact]
    public async Task GetAsync_Should_Return_Filtered_AddressesEntities()
    {
        // Arrange
        var addressEntities = new List<AddressEntity>
            {
                new AddressEntity
                {
                    AddressId = 1,
                    City = "Stockholm",
                    PostalCode = "11234",
                    StreetName = "Sveavägen",
                    StreetNumber = "45A",
                },
                new AddressEntity
                {
                    AddressId = 2,
                    City = "Gothenburg",
                    PostalCode = "56789",
                    StreetName = "Main Street",
                    StreetNumber = "123",
                }
            };

        var mockAddressRepository = new Mock<IAddressRepository>();

        mockAddressRepository.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<AddressEntity, bool>>>(),
            It.IsAny<int>()))
            .ReturnsAsync((Expression<Func<AddressEntity, bool>> predicate, int take) =>
            {
                
                var filteredEntities = addressEntities.Where(predicate.Compile()).ToList();
                
                if (take > 0)
                {
                    filteredEntities = filteredEntities.Take(take).ToList();
                }
                return filteredEntities;
            });

        
        IAddressService addressService = new AddressService(mockAddressRepository.Object, mockLogs.Object);

        // Act
        var result = await addressService.GetAddressesAsync(d => d.AddressId == 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); 
        Assert.All(result, address => Assert.Equal("Stockholm", address.City)); 
    }



    [Fact]
    public async Task GetOneAsync_Should_Return_One_AddressEntity()
    {
        // Arrange
        var addressEntities = new List<AddressEntity>
        {
            new AddressEntity
            {
                AddressId = 1,
                City = "Stockholm",
                PostalCode = "11234",
                StreetName = "Sveavägen",
                StreetNumber = "45A",
            },
            new AddressEntity
            {
                AddressId = 2,
                City = "Gothenburg",
                PostalCode = "56789",
                StreetName = "Main Street",
                StreetNumber = "123",
            }
        };

        var mockAddressRepository = new Mock<IAddressRepository>();

        
        mockAddressRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                    .ReturnsAsync((Expression<Func<AddressEntity, bool>> predicate) =>
                    {
                        
                        var result = addressEntities.FirstOrDefault(predicate.Compile());
                        return result;
                    });

        // Act
        
        IAddressService addressService = new AddressService(mockAddressRepository.Object, new Logs("test.log", true));
        var result = await addressService.GetOneAddressAsync(d => d.AddressId == 1);

        // Assert
 
        Assert.NotNull(result);
        Assert.Equal(1, result.AddressId); 
    }





    [Fact]
    public async Task UpdateAsync_Should_Update_AddressEntity()
    {
        // Arrange
        var updatedAddressDto = new UpdatedAddressDto
        {
            AddressId = 1,
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        var existingAddressEntity = new AddressEntity
        {
            AddressId = 1, 
            City = "Gothenburg",
            PostalCode = "56789",
            StreetName = "Main Street",
            StreetNumber = "123",
        };

        var mockAddressRepository = new Mock<IAddressRepository>();
        mockAddressRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                            .ReturnsAsync(existingAddressEntity);

        mockAddressRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>(), It.IsAny<AddressEntity>()))
                            .ReturnsAsync((Expression<Func<AddressEntity, bool>> predicate, AddressEntity updatedEntity) =>
                            {
                               
                                existingAddressEntity.StreetName = updatedEntity.StreetName;
                                existingAddressEntity.StreetNumber = updatedEntity.StreetNumber;
                                existingAddressEntity.PostalCode = updatedEntity.PostalCode;
                                existingAddressEntity.City = updatedEntity.City;

                               
                                return existingAddressEntity;
                            });

        IAddressService addressService = new AddressService(mockAddressRepository.Object, new Logs("test.log", true));

        // Act
        var result = await addressService.UpdateAddressAsync(1, updatedAddressDto);

        // Debugging statements
        Debug.WriteLine($"Existing Address Entity: {existingAddressEntity}");
        Debug.WriteLine($"Updated Address DTO: {updatedAddressDto}");
        Debug.WriteLine($"Result after update: {result}");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedAddressDto.AddressId, result.AddressId); 
        Assert.Equal(updatedAddressDto.City, result.City); 
    }



    [Fact]
    public async Task DeleteAsync_Should_Delete_AddressEntity()
    {
        // Arrange
        var addressEntity = new List<AddressEntity>
        {
            new AddressEntity
            {
                City = "Stockholm",
                PostalCode = "11234",
                StreetName = "Sveavägen",
                StreetNumber = "45A",
            },
            
        };

        var mockAddressRepository = new Mock<IAddressRepository>();
        mockAddressRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                                .ReturnsAsync(true); 

        IAddressService addressService = new AddressService(mockAddressRepository.Object, new Logs("test.log", true));

        // Act
        var result = await addressService.DeleteAddressAsync(d => d.AddressId == 1); 

        // Assert
        Assert.True(result); 

        
        mockAddressRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()), Times.Once);
    }


}
