using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Services;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace Infrastructure.Tests.Services;

public class DepartmentService_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

   
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockRepository = new Mock<IDepartmentRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<DepartmentEntity>()))
            .ThrowsAsync(new Exception("Simulated exception"));

        IDepartmentService departmentService = new DepartmentService(mockRepository.Object, mockLogs.Object);

        // Act
        var result = await departmentService.CreateDepartmentAsync("IT");

        // Assert
        Assert.Null(result);

        
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }



    [Fact]
    public async Task GetAllAsync_Should_Return_All_DepartmentDtos()
    {
        // Arrange
        var departmentEntities = new List<DepartmentEntity>
        {
            new DepartmentEntity { DepartmentName = "Finance" },
            new DepartmentEntity { DepartmentName = "IT" },
        };

        var departmentDtos = departmentEntities.Select(entity => new DepartmentDto { DepartmentName = entity.DepartmentName });

        
        var mockRepository = new Mock<IDepartmentRepository>();

        
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(departmentEntities);

        var departmentRepository = mockRepository.Object;

        IDepartmentService departmentService = new DepartmentService(departmentRepository, mockLogs.Object);

        // Act
        var result = await departmentService.GetAllDepartmentAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(departmentDtos.Count(), result.Count());
        Assert.IsAssignableFrom<IEnumerable<DepartmentDto>>(result); 
        foreach (var dto in departmentDtos)
        {
            Assert.Contains(result, d => d.DepartmentName == dto.DepartmentName);
        }
    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_DepartmentEntities()
    {
        // Arrange
        var departmentEntities = new List<DepartmentEntity>
        {
            new DepartmentEntity { DepartmentName = "Finance" },
            new DepartmentEntity { DepartmentName = "IT" },
        };

        
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();

        
        mockDepartmentRepository.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<DepartmentEntity, bool>>>(),
            It.IsAny<int>()))
            .ReturnsAsync((Expression<Func<DepartmentEntity, bool>> predicate, int take) =>
            {
               
                var filteredEntities = departmentEntities.Where(predicate.Compile()).ToList();
                
                if (take > 0)
                {
                    filteredEntities = filteredEntities.Take(take).ToList();
                }
                return filteredEntities;
            });

        
        IDepartmentService departmentService = new DepartmentService(mockDepartmentRepository.Object, new Logs("test.log", true));

        // Act
        var result = await departmentService.GetDepartmentsAsync(d => d.DepartmentName == "Finance", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count()); 
        Assert.All(result, department => Assert.Equal("Finance", department.DepartmentName)); // Ensure all entities have the correct department name
    }



    [Fact]
    public async Task GetOneAsync_Should_Return_One_DepartmentEntity()
    {
        // Arrange
        var mockDepartmentRepository = new Mock<IDepartmentRepository>();

        var departmentEntities = new List<DepartmentEntity>
        {
            new DepartmentEntity { DepartmentName = "Finance" },
            new DepartmentEntity { DepartmentName = "IT" },
        };

        
        mockDepartmentRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<DepartmentEntity, bool>>>()))
                                .ReturnsAsync((Expression<Func<DepartmentEntity, bool>> predicate) =>
                                {
                                    return departmentEntities.FirstOrDefault(predicate.Compile());
                                });

        // Act
        
        IDepartmentService departmentService = new DepartmentService(mockDepartmentRepository.Object, new Logs("test.log", true));
        var result = await departmentService.GetOneDepartmentAsync(d => d.DepartmentName == "Finance");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Finance", result.DepartmentName);
    }



    [Fact]
    public async Task UpdateAsync_Should_Update_DepartmentEntity()
    {
        // Arrange
        var departmentEntity = new DepartmentEntity { DepartmentName = "Finance" };
        _employeeDbContext.Departments.Add(departmentEntity);
        await _employeeDbContext.SaveChangesAsync();

        IDepartmentRepository departmentRepository = new DepartmentRepository(_employeeDbContext, new Logs("test.log", true));
        IDepartmentService departmentService = new DepartmentService(departmentRepository, new Logs("test.log", true));

        // Act
        var updatedEntity = await departmentService.UpdateDepartmentAsync(new UpdatedDepartmentDto { Id = departmentEntity.DepartmentId, DepartmentName = "Accounting" });

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal("Accounting", updatedEntity.DepartmentName);

        
        var departmentInDatabase = await _employeeDbContext.Departments.FindAsync(departmentEntity.DepartmentId);
        Assert.NotNull(departmentInDatabase);
        Assert.Equal("Accounting", departmentInDatabase.DepartmentName);
    }



    [Fact]
    public async Task DeleteAsync_Should_Delete_DepartmentEntity()
    {
        // Arrange
        var departmentEntity = new DepartmentEntity { DepartmentName = "Finance" };

        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        mockDepartmentRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<DepartmentEntity, bool>>>()))
                                .ReturnsAsync(true); 

        IDepartmentService departmentService = new DepartmentService(mockDepartmentRepository.Object, new Logs("test.log", true));

        // Act
        var result = await departmentService.DeleteDepartmentAsync(d => d.DepartmentName == "Finance");

        // Assert
        Assert.True(result); 

        
        mockDepartmentRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Expression<Func<DepartmentEntity, bool>>>()), Times.Once);
    }



}
