using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;

namespace Infrastructure.Tests.Repositories;

public class EmployeeAddressRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<EmployeeAddressEntity>>(_employeeDbContext, mockLogs.Object);
        var employeeAddressesRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);

        // Configure the mock to throw an exception when CreateAsync is called
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<EmployeeAddressEntity>()))
            .ThrowsAsync(new Exception("Simulated exception"));

        // Act
        var result = await employeeAddressesRepository.CreateAsync(null!); // Pass a null entity to trigger an exception

        // Assert
        Assert.Null(result);

        // Verify that the exception is logged
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_EmployeeAddressesEntities()
    {


        // Arrange
        var employeeaddressesEntities = new List<EmployeeAddressEntity>
        {
            new EmployeeAddressEntity { EmployeeId = 1, AddressId = 1 },
            new EmployeeAddressEntity { EmployeeId = 2, AddressId = 2 },
        };

       
        var mockRepository = new Mock<EmployeeAddressRepository>(_employeeDbContext, mockLogs.Object);

       
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(employeeaddressesEntities);

        var employeeaddressesRepository = mockRepository.Object;

        // Act
        var result = await employeeaddressesRepository.GetAllAsync();


        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeaddressesEntities.Count, result.Count()); 
        Assert.IsAssignableFrom<IEnumerable<EmployeeAddressEntity>>(result);
        foreach (var entity in employeeaddressesEntities)
        {
            Assert.Contains(result, e => e.EmployeeId == entity.EmployeeId && e.AddressId == entity.AddressId);
        }
    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_EmployeeAddressEntities()
    {
        // Arrange
        var employeeaddressesEntities = new List<EmployeeAddressEntity>
        {
            new EmployeeAddressEntity { EmployeeId = 1, AddressId = 1 },
            new EmployeeAddressEntity { EmployeeId = 2, AddressId = 2 },
        };

        _employeeDbContext.EmployeeAddresses.AddRange(employeeaddressesEntities);
        await _employeeDbContext.SaveChangesAsync();

        var employeeaddressesRepository = new EmployeeAddressRepository(_employeeDbContext, mockLogs.Object);

        // Act

        var result = await employeeaddressesRepository.GetAsync(d => d.EmployeeId == 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); 
        Assert.IsAssignableFrom<IEnumerable<EmployeeAddressEntity>>(result); 
    }



    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var employeeaddressesEntities = new List<EmployeeAddressEntity>
        {
            new EmployeeAddressEntity { EmployeeId = 1, AddressId = 1 },
            new EmployeeAddressEntity { EmployeeId = 2, AddressId = 2 },
        };

        _employeeDbContext.EmployeeAddresses.AddRange(employeeaddressesEntities);
        await _employeeDbContext.SaveChangesAsync();


        var employeeAddressesRepository = new EmployeeAddressRepository(_employeeDbContext, mockLogs.Object);

        var employeeAddressesIdToFind = 1; 

        // Act
        var result = await employeeAddressesRepository.ExistsAsync(a => a.EmployeeId == employeeAddressesIdToFind);

        // Assert
        Assert.True(result);
    }



    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var employeeaddressesEntities = new List<EmployeeAddressEntity>
        {
            new EmployeeAddressEntity { EmployeeId = 1, AddressId = 1 },
            new EmployeeAddressEntity { EmployeeId = 2, AddressId = 2 },
        };

        _employeeDbContext.EmployeeAddresses.AddRange(employeeaddressesEntities);
        await _employeeDbContext.SaveChangesAsync();


        var employeeAddressesRepository = new EmployeeAddressRepository(_employeeDbContext, mockLogs.Object);

        var employeeAddressesIdToFind = 3; 

        // Act
        var result = await employeeAddressesRepository.ExistsAsync(a => a.EmployeeId == employeeAddressesIdToFind);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_EmployeeAddressEntities()
    {
 

        // Arrange
        var employeeAddressEntities = new List<EmployeeAddressEntity>
        {
            new EmployeeAddressEntity { EmployeeId = 1, AddressId = 1 },
            new EmployeeAddressEntity { EmployeeId = 2, AddressId = 2 },
        };

        _employeeDbContext.EmployeeAddresses.AddRange(employeeAddressEntities);
        await _employeeDbContext.SaveChangesAsync();

        var employeeAddressesRepository = new EmployeeAddressRepository(_employeeDbContext, mockLogs.Object);

        // Act
    

        var updatedEntity = new EmployeeAddressEntity { EmployeeId = 1, AddressId = 1 };


        // Assert
        try
        {
            var result = await employeeAddressesRepository.UpdateAsync(
                d => d.EmployeeId == 1 && d.AddressId == 1, 
                updatedEntity);

          
          
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
    public async Task DeleteAsync_Should_Delete_SkillEntities()
    {
   

        // Arrange
        var employeeAddressEntities = new List<EmployeeAddressEntity>
        {
            new EmployeeAddressEntity { EmployeeId = 1, AddressId = 1 },
            new EmployeeAddressEntity { EmployeeId = 2, AddressId = 2 },
        };

        _employeeDbContext.EmployeeAddresses.AddRange(employeeAddressEntities);
        await _employeeDbContext.SaveChangesAsync();

        var salaryRepository = new EmployeeAddressRepository(_employeeDbContext, mockLogs.Object);

        // Act
        bool result = false;
        try
        {
            result = await salaryRepository.DeleteAsync(d => d.EmployeeId == 2);
  
        }
        catch (Exception ex)
        {
 
            throw; 
        }
        finally
        {
            
            await _employeeDbContext.SaveChangesAsync();
        }

        // Assert
        Assert.True(result);

        // Assert that the entity is not present in the database
        var deletedEntity = await _employeeDbContext.EmployeeAddresses.FirstOrDefaultAsync(e => e.EmployeeId == 2);
        Assert.Null(deletedEntity);
    }


}
