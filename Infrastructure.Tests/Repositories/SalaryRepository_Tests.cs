using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;

namespace Infrastructure.Tests.Repositories;

public class SalaryRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockGenericRepository = new Mock<GenericRepository<SalaryEntity>>(_employeeDbContext, mockLogs.Object);
        var salaryRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);

        
        mockGenericRepository.Setup(repo => repo.CreateAsync(It.IsAny<SalaryEntity>()))
            .ThrowsAsync(new Exception("Simulated exception"));

        // Act
        var result = await salaryRepository.CreateAsync(null!); 

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_SalaryEntities()
    {
        

        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        _employeeDbContext.Salaries.AddRange(salaryEntities);
        await _employeeDbContext.SaveChangesAsync();

        var salaryRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);

        // Act
        var result = await salaryRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(salaryEntities.Count, result.Count()); 
        Assert.IsAssignableFrom<IEnumerable<SalaryEntity>>(result); 
        foreach (var entity in salaryEntities)
        {
            Assert.Contains(result, e => e.SalaryId == entity.SalaryId);
        }
    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_SalaryEntities()
    {
        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        _employeeDbContext.Salaries.AddRange(salaryEntities);
        await _employeeDbContext.SaveChangesAsync();

        var salaryRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);

        // Act
        
        var result = await salaryRepository.GetAsync(d => d.SalaryId == 1, 10); 

        // Debug
       

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); 
        Assert.IsAssignableFrom<IEnumerable<SalaryEntity>>(result);
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        _employeeDbContext.Salaries.AddRange(salaryEntities);
        await _employeeDbContext.SaveChangesAsync();

        var salaryRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);

        var salaryIdToFind = 1; 

        // Act
        var result = await salaryRepository.ExistsAsync(a => a.SalaryId == salaryIdToFind);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        _employeeDbContext.Salaries.AddRange(salaryEntities);
        await _employeeDbContext.SaveChangesAsync();

        var salaryRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);

        var salaryIdToFind = 3; 

        // Act
        var result = await salaryRepository.ExistsAsync(a => a.SalaryId == salaryIdToFind);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_PositionEntities()
    {
        

        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        _employeeDbContext.Salaries.AddRange(salaryEntities);
        await _employeeDbContext.SaveChangesAsync();

        var salaryRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);

        // Act
        

        var updatedEntity = new SalaryEntity { SalaryId = 1, Amount = 27000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) };
    

        try
        {
            var result = await salaryRepository.UpdateAsync(
                d => d.SalaryId == 1,
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
        Debug.WriteLine("Debug Message: This line is executed.");

        // Arrange
        var salaryEntities = new List<SalaryEntity>
    {
        new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
        new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
    };

        _employeeDbContext.Salaries.AddRange(salaryEntities);
        await _employeeDbContext.SaveChangesAsync();

        var salaryRepository = new SalaryRepository(_employeeDbContext, mockLogs.Object);


        // Act
        bool result = false;
        try
        {
            result = await salaryRepository.DeleteAsync(d => d.SalaryId == 2);
   
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

       
        var deletedEntity = await _employeeDbContext.Salaries.FirstOrDefaultAsync(e => e.SalaryId == 2);
        Assert.Null(deletedEntity);
    }


}
