using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Tests.Repositories;

public class GenericRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    private readonly Mock<ILogs> mockLogs = new();


    [Fact]
    public async Task CreateAsync_WithValidEntity_ShouldReturnEntity()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = mockRepository.Object;

        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<AddressEntity>()))
            .ReturnsAsync(addressEntity); 

        // Act
        var result = await addressRepository.CreateAsync(addressEntity);

       

        
        var entitiesInDatabase = await _employeeDbContext.Addresses.ToListAsync();
        

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Stockholm", result.City); 
        Assert.Equal("11234", result.PostalCode); 
        Assert.Equal("Sveavägen", result.StreetName);
        Assert.Equal("45A", result.StreetNumber); 
    }






    [Fact]
    public async Task GetAllAsync_WithEntitiesInDatabase_ShouldReturnEntities()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = mockRepository.Object;

        var entities = new List<AddressEntity>
        {
            new AddressEntity { AddressId = 1 },
            new AddressEntity { AddressId = 2 },
            new AddressEntity { AddressId = 3 }
        };

        
        mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(entities);

        // Act
        var result = await addressRepository.GetAllAsync();


        // Assert
        Assert.NotNull(result);
        Assert.Equal(entities.Count, result.Count());
        foreach (var entity in entities)
        {
            Assert.Contains(result, e => e.AddressId == entity.AddressId);
        }

    }





    [Fact]
    public async Task UpdateAsync_WithValidEntity_ShouldReturnUpdatedEntity()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = mockRepository.Object;

        
        var initialEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A"
        };
        _employeeDbContext.Addresses.Add(initialEntity);
        await _employeeDbContext.SaveChangesAsync();

        
        var updatedEntity = new AddressEntity
        {
            City = "Göteborg",
            PostalCode = "12345",
            StreetName = "Kungsportsavenyn",
            StreetNumber = "1"
        };

        
        mockRepository.Setup(repo => repo.UpdateAsync(
                It.IsAny<Expression<Func<AddressEntity, bool>>>(), // Accept any predicate
                It.IsAny<AddressEntity>())) // Accept any updated entity
            .ReturnsAsync(updatedEntity); // Return the updated entity as the result

        // Act
        var result = await addressRepository.UpdateAsync(
            d => d.City == initialEntity.City &&
                 d.PostalCode == initialEntity.PostalCode &&
                 d.StreetName == initialEntity.StreetName &&
                 d.StreetNumber == initialEntity.StreetNumber, // Specify the predicate to find the entity to update
            updatedEntity); // Pass the updated entity

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedEntity.City, result.City);
        Assert.Equal(updatedEntity.PostalCode, result.PostalCode);
        Assert.Equal(updatedEntity.StreetName, result.StreetName);
        Assert.Equal(updatedEntity.StreetNumber, result.StreetNumber);
    }



    [Fact]
    public async Task DeleteAsync_WithValidEntity_ShouldReturnTrue()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = mockRepository.Object;

        var addressEntity = new AddressEntity
        {
            AddressId = 1,
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A"
        };

        
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<AddressEntity>()))
            .ReturnsAsync((AddressEntity entity) =>
            {
                
                return entity;
            });

        
        mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var createdEntity = await addressRepository.CreateAsync(addressEntity);
        var result = await addressRepository.DeleteAsync(e => e.AddressId == createdEntity.AddressId); 

        // Assert
        Assert.True(result);
    }




    [Fact]
    public async Task GetOneAsync_WithValidPredicate_ShouldReturnEntity()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = mockRepository.Object;

        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        mockRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                      .ReturnsAsync(addressEntity);

        // Act
        var result = await addressRepository.GetOneAsync(a => a.AddressId == addressEntity.AddressId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Stockholm", result.City);
    }


    [Fact]
    public async Task ExistsAsync_WithValidPredicate_ShouldReturnTrue()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = mockRepository.Object;

       
        mockRepository.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                      .ReturnsAsync(true);

        // Act
        var result = await addressRepository.ExistsAsync(e => e.AddressId == 1);

        // Assert
        Assert.True(result);
    }

}

