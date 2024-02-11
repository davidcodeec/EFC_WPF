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

public class PositionService_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
           .UseInMemoryDatabase($"{Guid.NewGuid()}")
           .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var positionEntities = new List<PositionEntity>
        {
            new PositionEntity { PositionId = 1, PositionName = "CEO" },
            new PositionEntity { PositionId = 2, PositionName = "Manager" },
        };

        var mockRepository = new Mock<IPositionRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<PositionEntity>()))
                      .ThrowsAsync(new Exception("Simulated exception"));

        var positionRepository = mockRepository.Object;

        IPositionService positionService = new PositionService(positionRepository, mockLogs.Object);

        // Act
        var result = await positionService.CreatePositionAsync("CEO");

        // Assert
        Assert.Null(result);

        
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }




    [Fact]
    public async Task GetAllAsync_Should_Return_All_PositionDtos()
    {
        // Arrange
        var positionEntities = new List<PositionEntity>
        {
            new PositionEntity { PositionName = "CEO" },
            new PositionEntity { PositionName = "Manager" },
        };

        var positionDtos = positionEntities.Select(entity => new PositionDto { PositionName = entity.PositionName });

        
        var mockRepository = new Mock<IPositionRepository>();

        
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(positionEntities);

        var posiionRepository = mockRepository.Object;

        IPositionService positionService = new PositionService(posiionRepository, mockLogs.Object);

        // Act
        var result = await positionService.GetAllPositionAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.All(dto => dto is PositionDto));
    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_PositionEntities()
    {
        // Arrange
        var positionEntities = new List<PositionEntity>
        {
            new PositionEntity { PositionName = "CEO" },
            new PositionEntity { PositionName = "Manager" },
        };

        
        var mockPositionRepository = new Mock<IPositionRepository>();

        
        mockPositionRepository.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<PositionEntity, bool>>>(),
            It.IsAny<int>()))
            .ReturnsAsync((Expression<Func<PositionEntity, bool>> predicate, int take) =>
            {
                
                var filteredEntities = positionEntities.Where(predicate.Compile()).ToList();
                
                if (take > 0)
                {
                    filteredEntities = filteredEntities.Take(take).ToList();
                }
                return filteredEntities;
            });

        
        IPositionService positionService = new PositionService(mockPositionRepository.Object, new Logs("test.log", true));

        // Act
        var result = await positionService.GetPositionsAsync(d => d.PositionName == "CEO", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count()); 
        Assert.All(result, position => Assert.Equal("CEO", position.PositionName)); // Ensure all entities have the correct department name
    }


    [Fact]
    public async Task GetOneAsync_Should_Return_One_PositionEntity()
    {
        // Arrange
        var mockPositionRepository = new Mock<IPositionRepository>();

        var positionEntities = new List<PositionEntity>
        {
            new PositionEntity { PositionName = "CEO" },
            new PositionEntity { PositionName = "Manager" },
        };


        
        mockPositionRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()))
                                .ReturnsAsync((Expression<Func<PositionEntity, bool>> predicate) =>
                                {
                                    return positionEntities.FirstOrDefault(predicate.Compile());
                                });

        // Act
        
        IPositionService positionervice = new PositionService(mockPositionRepository.Object, new Logs("test.log", true));
        var result = await positionervice.GetOnePositionAsync(d => d.PositionName == "CEO");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CEO", result.PositionName);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_PositionEntity()
    {
        // Arrange
        var positionEntity = new PositionEntity { PositionName = "CEO" };
        _employeeDbContext.Positions.Add(positionEntity);
        await _employeeDbContext.SaveChangesAsync();

        IPositionRepository positionRepository = new PositionRepository(_employeeDbContext, new Logs("test.log", true));
        IPositionService positionService = new PositionService(positionRepository, new Logs("test.log", true));

        // Act
        var updatedEntity = await positionService.UpdatePostionAsync(new UpdatedPositionDto { Id = positionEntity.PositionId, PositionName = "Manager" });

        // Assert
        Assert.NotNull(updatedEntity); 
        Assert.Equal("Manager", updatedEntity.PositionName); 

        
        var positionInDatabase = await _employeeDbContext.Positions.FindAsync(positionEntity.PositionId);
        Assert.NotNull(positionInDatabase);
        Assert.Equal("Manager", positionInDatabase.PositionName);
    }


    [Fact]
    public async Task DeleteAsync_Should_Delete_PositionEntity()
    {
        // Arrange
        var positionEntity = new PositionEntity { PositionName = "CEO" };

        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()))
                                .ReturnsAsync(true); 

        IPositionService positionService = new PositionService(mockPositionRepository.Object, new Logs("test.log", true));

        // Act
        var result = await positionService.DeletePositionAsync(d => d.PositionName == "CEO"); 

        // Assert
        Assert.True(result); 

        
        mockPositionRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()), Times.Once);
    }

}
