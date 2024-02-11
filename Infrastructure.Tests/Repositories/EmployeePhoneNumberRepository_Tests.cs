using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Tests.Repositories;

public class EmployeePhoneNumberRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<EmployeePhoneNumberEntity>>(_employeeDbContext, mockLogs.Object);
        var employeePhoneNumbersRepository = new EmployeePhoneNumberRepository(_employeeDbContext, mockLogs.Object);

        
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<EmployeePhoneNumberEntity>()))
            .ThrowsAsync(new Exception("Simulated exception"));

        // Act
        var result = await employeePhoneNumbersRepository.CreateAsync(null!); 

        // Assert
        Assert.Null(result);

        
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }




    [Fact]
    public async Task GetAllAsync_Should_Return_All_EmployeePhoneNumberEntities()
    {
        // Arrange
         var employeePhoneNumbersEntities = new List<EmployeePhoneNumberEntity>
        {
            new EmployeePhoneNumberEntity { Id = 1, PhoneNumber = "123-456-7890", EmployeeId = 1 },
            new EmployeePhoneNumberEntity { Id = 2, PhoneNumber = "987-654-3210", EmployeeId = 2 },
        };

        
        var mockRepository = new Mock<EmployeePhoneNumberRepository>(_employeeDbContext, mockLogs.Object);

        
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(employeePhoneNumbersEntities);

        var employeePhoneNumbersRepository = mockRepository.Object;

        // Act
        var result = await employeePhoneNumbersRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeePhoneNumbersEntities.Count, result.Count());
        Assert.IsAssignableFrom<IEnumerable<EmployeePhoneNumberEntity>>(result);
        foreach (var entity in employeePhoneNumbersEntities)
        {
            Assert.Contains(result, e => e.Id == entity.Id && e.PhoneNumber == entity.PhoneNumber);
        }
    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_EmployeePhoneNumberEntities()
    {
        // Arrange
        var employeePhoneNumbersEntities = new List<EmployeePhoneNumberEntity>
    {
        new EmployeePhoneNumberEntity { Id = 1, PhoneNumber = "123-456-7890", EmployeeId = 1 },
        new EmployeePhoneNumberEntity { Id = 2, PhoneNumber = "987-654-3210", EmployeeId = 2 },
    };

        
        var mockRepository = new Mock<EmployeePhoneNumberRepository>(_employeeDbContext, mockLogs.Object);

        
        mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<EmployeePhoneNumberEntity, bool>>>(), It.IsAny<int>()))
                      .ReturnsAsync((Expression<Func<EmployeePhoneNumberEntity, bool>> predicate, int take) =>
                      {
                          
                          var filteredEntities = employeePhoneNumbersEntities.AsQueryable().Where(predicate).Take(take);
                          return filteredEntities.ToList();
                      });

        var employeePhoneNumberRepository = mockRepository.Object;

        // Act
        var result = await employeePhoneNumberRepository.GetAsync(d => d.Id == 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); 
        Assert.IsAssignableFrom<IEnumerable<EmployeePhoneNumberEntity>>(result); 
    }



    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var employeePhoneNumbersEntities = new List<EmployeePhoneNumberEntity>
        {
            new EmployeePhoneNumberEntity { Id = 1, PhoneNumber = "123-456-7890", EmployeeId = 1 },
            new EmployeePhoneNumberEntity { Id = 2, PhoneNumber = "987-654-3210", EmployeeId = 2 },
        };

        
        var mockRepository = new Mock<EmployeePhoneNumberRepository>(_employeeDbContext, mockLogs.Object);

        _employeeDbContext.EmployeePhoneNumbers.AddRange(employeePhoneNumbersEntities);
        await _employeeDbContext.SaveChangesAsync();


        var employeePhoneNumbersRepository = new EmployeePhoneNumberRepository(_employeeDbContext, mockLogs.Object);

        var employeePhoneNumbersIdToFind = 1; 

        // Act
        var result = await employeePhoneNumbersRepository.ExistsAsync(a => a.EmployeeId == employeePhoneNumbersIdToFind);

        // Assert
        Assert.True(result);
    }



    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var employeePhoneNumbersEntities = new List<EmployeePhoneNumberEntity>
        {
            new EmployeePhoneNumberEntity { Id = 1, PhoneNumber = "123-456-7890", EmployeeId = 1 },
            new EmployeePhoneNumberEntity { Id = 2, PhoneNumber = "987-654-3210", EmployeeId = 2 },
        };

        
        var mockRepository = new Mock<EmployeePhoneNumberRepository>(_employeeDbContext, mockLogs.Object);

        _employeeDbContext.EmployeePhoneNumbers.AddRange(employeePhoneNumbersEntities);
        await _employeeDbContext.SaveChangesAsync();


        var employeePhoneNumbersRepository = new EmployeePhoneNumberRepository(_employeeDbContext, mockLogs.Object);

        var employeeAddressesIdToFind = 3; 

        // Act
        var result = await employeePhoneNumbersRepository.ExistsAsync(a => a.EmployeeId == employeeAddressesIdToFind);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_EmployeePhoneNumberEntities()
    {
        // Arrange
        var employeePhoneNumbersEntities = new List<EmployeePhoneNumberEntity>
        {
            new EmployeePhoneNumberEntity { Id = 1, PhoneNumber = "123-456-7890", EmployeeId = 1 },
            new EmployeePhoneNumberEntity { Id = 2, PhoneNumber = "987-654-3210", EmployeeId = 2 },
        };

        var mockRepository = new Mock<EmployeePhoneNumberRepository>(_employeeDbContext, mockLogs.Object);
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<EmployeePhoneNumberEntity, bool>>>(), It.IsAny<EmployeePhoneNumberEntity>()))
                      .ReturnsAsync((Expression<Func<EmployeePhoneNumberEntity, bool>> predicate, EmployeePhoneNumberEntity updatedEntity) =>
                      {
                         
                          return updatedEntity;
                      });

        var employeePhoneNumbersRepository = mockRepository.Object;

        var updatedEntity = new EmployeePhoneNumberEntity { Id = 1, PhoneNumber = "707-456-7890", EmployeeId = 1 };

        // Act
        var result = await employeePhoneNumbersRepository.UpdateAsync(
            d => d.Id == 1 && d.EmployeeId == updatedEntity.EmployeeId, 
            updatedEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedEntity.Id, result.Id);
        Assert.Equal(updatedEntity.PhoneNumber, result.PhoneNumber); 
    }


    [Fact]
    public async Task DeleteAsync_Should_Delete_EmployeePhoneNumberEntities()
    {
        var employeePhoneNumbersEntities = new List<EmployeePhoneNumberEntity>
    {
        new EmployeePhoneNumberEntity { Id = 1, PhoneNumber = "123-456-7890", EmployeeId = 1 },
        new EmployeePhoneNumberEntity { Id = 2, PhoneNumber = "987-654-3210", EmployeeId = 2 },
    };

       
        var mockRepository = new Mock<EmployeePhoneNumberRepository>(_employeeDbContext, mockLogs.Object);

        _employeeDbContext.EmployeePhoneNumbers.AddRange(employeePhoneNumbersEntities);
        await _employeeDbContext.SaveChangesAsync();

        var employeePhoneNumbersRepository = new EmployeePhoneNumberRepository(_employeeDbContext, mockLogs.Object);

        // Act
        bool result = false;
        try
        {
            result = await employeePhoneNumbersRepository.DeleteAsync(d => d.Id == 2);
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

        
        var deletedEntity = await _employeeDbContext.EmployeePhoneNumbers.FirstOrDefaultAsync(e => e.Id == 2);
        Assert.Null(deletedEntity);
    }

}
