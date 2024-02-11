using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;

namespace Infrastructure.Tests.Repositories;

public class PositionRepository_Tests
{

    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockGenericRepository = new Mock<GenericRepository<PositionEntity>>(_employeeDbContext, mockLogs.Object);
        var positionRepository = new PositionRepository(_employeeDbContext, mockLogs.Object);

       
        mockGenericRepository.Setup(repo => repo.CreateAsync(It.IsAny<PositionEntity>()))
            .ThrowsAsync(new Exception("Simulated exception"));

        // Act
        var result = await positionRepository.CreateAsync(null!); 

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_PositionEntities()
    {


        // Arrange
        var positionEntities = new List<PositionEntity>
        {
            new() { PositionId = 1, PositionName = "CEO" },
            new() { PositionId = 2, PositionName = "Manager" },
        };

        _employeeDbContext.Positions.AddRange(positionEntities);
        await _employeeDbContext.SaveChangesAsync();

        var positionRepository = new PositionRepository(_employeeDbContext, mockLogs.Object); 

        // Act
        var result = await positionRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(positionEntities.Count, result.Count()); // Check if the count matches
        Assert.IsAssignableFrom<IEnumerable<PositionEntity>>(result); // Check the type of the result
        foreach (var entity in positionEntities)
        {
            Assert.Contains(result, e => e.PositionId == entity.PositionId);
        }
    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_PositionEntities()
    {
        // Arrange
        var positionRepository = new PositionRepository(_employeeDbContext, mockLogs.Object);

        var positionEntities = new List<PositionEntity>
        {
            new() { PositionId = 1, PositionName = "CEO" },
            new() { PositionId = 2, PositionName = "Manager" },
        };

        _employeeDbContext.Positions.AddRange(positionEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
  
        var result = await positionRepository.GetAsync(d => d.PositionName == "CEO", 10);

        // Debug
  

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); // Only one entity should match the predicate
        Assert.IsAssignableFrom<IEnumerable<PositionEntity>>(result); // Correct the type assertion
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var positionRepository = new PositionRepository(_employeeDbContext, mockLogs.Object);

        var positionEntities = new List<PositionEntity>
        {
            new() { PositionId = 1, PositionName = "CEO" },
            new() { PositionId = 2, PositionName = "Manager" },
        };

        _employeeDbContext.Positions.AddRange(positionEntities);
        await _employeeDbContext.SaveChangesAsync();

        var positionIdToFind = 1; // Choose an existing skill ID to find

        // Act
        var result = await positionRepository.ExistsAsync(a => a.PositionId == positionIdToFind);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var positionRepository = new PositionRepository(_employeeDbContext, mockLogs.Object);

        var positionEntities = new List<PositionEntity>
        {
            new() { PositionId = 1, PositionName = "CEO" },
            new() { PositionId = 2, PositionName = "Manager" },
        };

        _employeeDbContext.Positions.AddRange(positionEntities);
        await _employeeDbContext.SaveChangesAsync();

        var positionIdToFind = 3; 

        // Act
        var result = await positionRepository.ExistsAsync(a => a.PositionId == positionIdToFind);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_PositionEntities()
    {

        // Arrange
        var positionRepository = new PositionRepository(_employeeDbContext, mockLogs.Object);

            var positionEntities = new List<PositionEntity>
        {
            new() { PositionId = 1, PositionName = "CEO" },
            new() { PositionId = 2, PositionName = "Manager" },
        };

        _employeeDbContext.Positions.AddRange(positionEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
 

        var updatedEntity = new PositionEntity { PositionId = 1, PositionName = "Director" };

        

        try
        {
            var result = await positionRepository.UpdateAsync(
                d => d.PositionId == 1 && d.PositionName == "CEO", 
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
    public async Task DeleteAsync_Should_Delete_PositionEntities()
    {
 

        // Arrange
        var positionRepository = new PositionRepository(_employeeDbContext, mockLogs.Object);

        var positionEntities = new List<PositionEntity>
        {
            new() { PositionId = 1, PositionName = "CEO" },
            new() { PositionId = 2, PositionName = "Manager" },
        };

        
        _employeeDbContext.Positions.AddRange(positionEntities);
        await _employeeDbContext.SaveChangesAsync();

        
   

        // Act
        bool result = false;
        try
        {
            result = await positionRepository.DeleteAsync(d => d.PositionId == 2 && d.PositionName == "Manager");
            
  
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

        
        var entitiesInDatabase = await _employeeDbContext.Positions.ToListAsync();
        Assert.DoesNotContain(entitiesInDatabase.FirstOrDefault(e => e.PositionId == 2), entitiesInDatabase);
    }

}
