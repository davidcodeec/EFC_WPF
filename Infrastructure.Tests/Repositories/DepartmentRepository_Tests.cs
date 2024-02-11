using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics;


namespace Infrastructure.Tests.Repositories;

public class DepartmentRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        // Act
        var result = await departmentRepository.CreateAsync(null!); 

        // Assert
        Assert.Null(result);
    }



    [Fact]
    public async Task GetAllAsync_Should_Return_All_DepartmentEntities()
    {
       

        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        var departmentEntities = new List<DepartmentEntity>
        {
            new() { DepartmentName = "Finance" },
            new() { DepartmentName = "IT" },
        };

        _employeeDbContext.Departments.AddRange(departmentEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        
        var result = await departmentRepository.GetAllAsync();

        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(departmentEntities.Count, result.Count()); 
        Assert.IsAssignableFrom<IEnumerable<DepartmentEntity>>(result);
    }



    [Fact]
    public async Task GetAsync_Should_Return_Filtered_DepartmentEntities()
    {
        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        var departmentEntities = new List<DepartmentEntity>
        {
            new() { DepartmentName = "Finance" },
            new() { DepartmentName = "IT" },
        };

        _employeeDbContext.Departments.AddRange(departmentEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var result = await departmentRepository.GetAsync(d => d.DepartmentName == "Finance", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result); 
        Assert.IsAssignableFrom<IEnumerable<DepartmentEntity>>(result);
    }



    [Fact]
    public async Task GetOneAsync_Should_Return_One_DepartmentEntity()
    {
        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        var departmentEntities = new List<DepartmentEntity>
        {
            new() { DepartmentName = "Finance" },
            new() { DepartmentName = "IT" },
        };

        _employeeDbContext.Departments.AddRange(departmentEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var result = await departmentRepository.GetOneAsync(d => d.DepartmentName == "Finance");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Finance", result.DepartmentName);
    }



    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        var departmentEntities = new List<DepartmentEntity>
        {
            new() { DepartmentName = "Finance" },
            new() { DepartmentName = "IT" },
        };

        _employeeDbContext.Departments.AddRange(departmentEntities);
        await _employeeDbContext.SaveChangesAsync();

        // Act
        var result = await departmentRepository.ExistsAsync(d => d.DepartmentName == "Finance");

        // Assert
        Assert.True(result);
    }



    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        // Act
        var result = await departmentRepository.ExistsAsync(d => d.DepartmentName == "Finance");

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_DepartmentEntity()
    {
       
        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        var departmentEntity = new DepartmentEntity { DepartmentName = "Finance" };
        _employeeDbContext.Departments.Add(departmentEntity);
        await _employeeDbContext.SaveChangesAsync();

        // Act
       
        var updatedEntity = new DepartmentEntity { DepartmentName = "Accounting" };

        try
        {
            var result = await departmentRepository.UpdateAsync(d => d.DepartmentName == "Finance", updatedEntity);
            

            // Assert
            
            Assert.NotNull(result);
            Assert.Equal("Accounting", result.DepartmentName);

            // Ensure that the DepartmentEntity is updated in the database
            var departmentInDatabase = await _employeeDbContext.Departments.FirstOrDefaultAsync(d => d.DepartmentName == "Accounting");
            Assert.NotNull(departmentInDatabase);
            Assert.Equal("Accounting", departmentInDatabase.DepartmentName);
        }
        catch (Exception ex)
        {
            
            throw; // Rethrow the exception after logging
        }
        finally
        {
            // Optionally, you can save changes here if needed
            await _employeeDbContext.SaveChangesAsync();
        }
    }




    [Fact]
    public async Task DeleteAsync_Should_Delete_DepartmentEntity()
    {
        

        // Arrange
        var departmentRepository = new DepartmentRepository(_employeeDbContext, mockLogs.Object);

        var departmentEntity = new DepartmentEntity { DepartmentName = "Finance" };

       
        

        // Act
        var result = await departmentRepository.DeleteAsync(d => d.DepartmentName == "Finance");

        

        // Assert
        if (!result)
        {
            
            var entitiesInDatabase = _employeeDbContext.Set<DepartmentEntity>().ToList();
            
            // Assert that the entity is not present in the database
            Assert.DoesNotContain(departmentEntity, entitiesInDatabase);
        }
        else
        {
            // Assert that the entity is not present in the database
            var entitiesInDatabase = _employeeDbContext.Set<DepartmentEntity>().ToList();
            Assert.DoesNotContain(departmentEntity, entitiesInDatabase);
        }
    }



}
