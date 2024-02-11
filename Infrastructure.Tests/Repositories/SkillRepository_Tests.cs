using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;

namespace Infrastructure.Tests.Repositories;

public class SkillRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockGenericRepository = new Mock<GenericRepository<SkillEntity>>(_employeeDbContext, mockLogs.Object);
        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        
        mockGenericRepository.Setup(repo => repo.CreateAsync(It.IsAny<SkillEntity>()))
            .ReturnsAsync((SkillEntity entity) => entity);

        // Act
        var result = await skillRepository.CreateAsync(null!); 

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_SkillEntities()
    {

        // Arrange
        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        _employeeDbContext.Skills.AddRange(skillEntities);
        await _employeeDbContext.SaveChangesAsync();

        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        // Act
        var result = await skillRepository.GetAllAsync();


        // Assert
        Assert.NotNull(result);
        Assert.Equal(skillEntities.Count, result.Count()); 
        Assert.IsAssignableFrom<IEnumerable<SkillEntity>>(result);
        foreach (var entity in skillEntities)
        {
            Assert.Contains(result, e => e.SkillId == entity.SkillId);
        }
    }



    [Fact]
    public async Task GetAsync_Should_Return_Filtered_SkillEntities()
    {
        // Arrange
        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        _employeeDbContext.Skills.AddRange(skillEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        Debug.WriteLine("Debugging: Calling GetAsync method.");
        var result = await skillRepository.GetAsync(d => d.SkillName == "Programming", 10);

        // Debug
        Debug.WriteLine($"Debugging: Result count: {(result != null ? result.Count() : 0)}");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); 
        Assert.IsAssignableFrom<IEnumerable<SkillEntity>>(result);
    }


    [Fact]
    public async Task GetOneAsync_Should_Return_One_SkillEntities()
    {
        // Arrange
        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        _employeeDbContext.Skills.AddRange(skillEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var skillIdToFind = 1; 
        var result = await skillRepository.GetOneAsync(a => a.SkillId == skillIdToFind);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Programming", result.SkillName);
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        _employeeDbContext.Skills.AddRange(skillEntities);
        await _employeeDbContext.SaveChangesAsync();

        var skillIdToFind = 1;

        // Act
        var result = await skillRepository.ExistsAsync(a => a.SkillId == skillIdToFind);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        _employeeDbContext.Skills.AddRange(skillEntities);
        await _employeeDbContext.SaveChangesAsync();

        var skillIdToFind = 3; 

        // Act
        var result = await skillRepository.ExistsAsync(a => a.SkillId == skillIdToFind);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_SkillEntities()
    {

        // Arrange
        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        _employeeDbContext.Skills.AddRange(skillEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act

        var updatedEntity = new SkillEntity { SkillId = 1, SkillName = "Networking" };

        try
        {
            var result = await skillRepository.UpdateAsync(
                d => d.SkillId == 1 && d.SkillName == "Programming",
                updatedEntity);



            // Assert
            Assert.NotNull(result);
            Assert.Equal("Networking", result.SkillName);
            Assert.Equal(1, result.SkillId);
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
        var skillRepository = new SkillRepository(_employeeDbContext, mockLogs.Object);

        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        
        _employeeDbContext.Skills.AddRange(skillEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        bool result = false;
        try
        {
            result = await skillRepository.DeleteAsync(d => d.SkillId == 2 && d.SkillName == "Networking");
            
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

        
        var entitiesInDatabase = await _employeeDbContext.Skills.ToListAsync();
        Assert.DoesNotContain(skillEntities[1], entitiesInDatabase);
    }



}





