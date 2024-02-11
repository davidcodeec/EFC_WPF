using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;


namespace Infrastructure.Tests.Repositories;

public class AddressRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Add_One_AddressEntity_To_Database_And_Return_Updated_AddressEntity()
    {
        // Arrange
        var mockGenericRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        mockGenericRepository.Setup(repo => repo.CreateAsync(It.IsAny<AddressEntity>()))
            .ReturnsAsync(addressEntity);

        // Act
        var result = await addressRepository.CreateAsync(addressEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressEntity.City, result.City);

        // Assert Database State
        var entitiesInDatabase = _employeeDbContext.Set<AddressEntity>().ToList();
        Assert.Contains(result, entitiesInDatabase);
    }


    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockGenericRepository = new Mock<GenericRepository<AddressEntity>>(_employeeDbContext, mockLogs.Object);
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        mockGenericRepository.Setup(repo => repo.CreateAsync(It.IsAny<AddressEntity>()))
            .Returns(Task.FromResult<AddressEntity?>(null));

        // Act
        var result = await addressRepository.CreateAsync(null!); // Pass a null entity to trigger an exception

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_AddressEntities()
    {
       

        // Arrange
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        var addressEntities = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        _employeeDbContext.Addresses.AddRange(addressEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        
        var result = await addressRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count()); // Corrected the expected count
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
    }




    [Fact]
    public async Task GetAsync_Should_Return_Filtered_AddressEntities()
    {
        // Arrange
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        var addressEntities = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        _employeeDbContext.Addresses.AddRange(addressEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var result = await addressRepository.GetAsync(d => d.City == "Stockholm", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); // Only one entity should match the predicate
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
    }



    [Fact]
    public async Task GetOneAsync_Should_Return_One_AddressEntities()
    {
        // Arrange
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        _employeeDbContext.Addresses.Add(addressEntity);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var result = await addressRepository.GetOneAsync(a => a.AddressId == addressEntity.AddressId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Stockholm", result.City);
    }




    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        _employeeDbContext.Addresses.Add(addressEntity); 
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var result = await addressRepository.ExistsAsync(d => d.City == "Stockholm");

        // Assert
        Assert.True(result);
    }




    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        // Act
        var result = await addressRepository.ExistsAsync(d => d.City == "Stockholm");

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_AddressEntities()
    {
        Debug.WriteLine("Debug Message: This line is executed.");

        // Arrange
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        _employeeDbContext.Addresses.Add(addressEntity);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        Debug.WriteLine("Debugging: Before UpdateAsync.");

        var updatedEntity = new AddressEntity
        {
            City = "Göteborg",
            PostalCode = "12345",
            StreetName = "Kungsportsavenyn",
            StreetNumber = "1",
        };

       

        try
        {
            var result = await addressRepository.UpdateAsync(
                d => d.StreetName == "Sveavägen" &&
                     d.StreetNumber == "45A" &&
                     d.PostalCode == "11234" &&
                     d.City == "Stockholm",
                updatedEntity);

           

            // Assert
           
            Assert.NotNull(result);
            Assert.Equal("Kungsportsavenyn", result.StreetName);
            Assert.Equal("1", result.StreetNumber);
            Assert.Equal("12345", result.PostalCode);
            Assert.Equal("Göteborg", result.City); 
        }
        catch (Exception ex)
        {
          
            throw; 
        }
        finally
        {
            
            await _employeeDbContext.SaveChangesAsync();
        }
    }


    [Fact]
    public async Task DeleteAsync_Should_Delete_AddressEntities()
    {
        Debug.WriteLine("Debug Message: This line is executed.");

        // Arrange
        var addressRepository = new AddressRepository(_employeeDbContext, mockLogs.Object);

        var addressEntity = new AddressEntity
        {
            City = "Stockholm",
            PostalCode = "11234",
            StreetName = "Sveavägen",
            StreetNumber = "45A",
        };

        // Add the entity to the database
        _employeeDbContext.Addresses.Add(addressEntity);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var result = await addressRepository.DeleteAsync(
                   d => d.StreetName == "Sveavägen" &&
                        d.StreetNumber == "45A" &&
                        d.PostalCode == "11234" &&
                        d.City == "Stockholm");

        // Assert
        Assert.True(result); // Assert that the deletion was successful

        // Assert that the entity is not present in the database
        var entitiesInDatabase = _employeeDbContext.Set<AddressEntity>().ToList();
        Assert.DoesNotContain(addressEntity, entitiesInDatabase);
    }



}