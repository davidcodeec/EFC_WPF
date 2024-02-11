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

public class SkillService_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
          .UseInMemoryDatabase($"{Guid.NewGuid()}")
          .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        var mockRepository = new Mock<ISkillRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<SkillEntity>()))
                      .ThrowsAsync(new Exception("Simulated exception"));

        var skillRepository = mockRepository.Object;

        ISkillService skillService = new SkillService(skillRepository, mockLogs.Object);

        // Act
        var result = await skillService.CreateSkillAsync("Programming");

        // Assert
        Assert.Null(result);

        
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_SkillDtos()
    {
        // Arrange
        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };

        var skillDtos = skillEntities.Select(entity => new SkillDto { SkillName = entity.SkillName });

        
        var mockRepository = new Mock<ISkillRepository>();

        
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(skillEntities);

        var skillRepository = mockRepository.Object;

        ISkillService skillService = new SkillService(skillRepository, mockLogs.Object);

        // Act
        var result = await skillService.GetAllSkillAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.All(dto => dto is SkillDto));

    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_PositionEntities()
    {
        // Arrange
        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };


        var mockSkillRepository = new Mock<ISkillRepository>();


        mockSkillRepository.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<SkillEntity, bool>>>(),
            It.IsAny<int>()))
            .ReturnsAsync((Expression<Func<SkillEntity, bool>> predicate, int take) =>
            {
                
                var filteredEntities = skillEntities.Where(predicate.Compile()).ToList();
                
                if (take > 0)
                {
                    filteredEntities = filteredEntities.Take(take).ToList();
                }
                return filteredEntities;
            });

        
        ISkillService skillService = new SkillService(mockSkillRepository.Object, new Logs("test.log", true));

        // Act
        var result = await skillService.GetSkillsAsync(d => d.SkillName == "Programming", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
        Assert.All(result, skill => Assert.Equal("Programming", skill.SkillName)); 
                                                                                  
    }


    [Fact]
    public async Task GetOneAsync_Should_Return_One_SkillEntity()
    {
        // Arrange
        var mockSkillRepository = new Mock<ISkillRepository>();

        var skillEntities = new List<SkillEntity>
        {
            new() { SkillId = 1, SkillName = "Programming" },
            new() { SkillId = 2, SkillName = "Networking" },
        };


        
        mockSkillRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<SkillEntity, bool>>>()))
                                .ReturnsAsync((Expression<Func<SkillEntity, bool>> predicate) =>
                                {
                                    return skillEntities.FirstOrDefault(predicate.Compile());
                                });

        // Act
        
        ISkillService skillervice = new SkillService(mockSkillRepository.Object, new Logs("test.log", true));
        var result = await skillervice.GetOneSkillAsync(d => d.SkillName == "Programming");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Programming", result.SkillName);

    }


    [Fact]
    public async Task UpdateAsync_Should_Update_SkillEntity()
    {
        // Arrange
        var skillEntity = new SkillEntity { SkillName = "Programming" };
        _employeeDbContext.Skills.Add(skillEntity);
        await _employeeDbContext.SaveChangesAsync();

        ISkillRepository skillRepository = new SkillRepository(_employeeDbContext, new Logs("test.log", true));
        ISkillService skillService = new SkillService(skillRepository, new Logs("test.log", true));

        // Act
        var updatedEntity = await skillService.UpdateSkillAsync(new UpdatedSkillDto { Id = skillEntity.SkillId, SkillName = "Networking" });

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal("Networking", updatedEntity.SkillName);

        var skillInDatabase = await _employeeDbContext.Skills.FindAsync(skillEntity.SkillId);
        Assert.NotNull(skillInDatabase);
        Assert.Equal("Networking", skillInDatabase.SkillName);

    }


    [Fact]
    public async Task DeleteAsync_Should_Delete_PositionEntity()
    {
        // Arrange
        var skillEntity = new SkillEntity { SkillName = "Programming" };

        var mockPositionRepository = new Mock<IPositionRepository>();
        mockPositionRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()))
                                .ReturnsAsync(true); 

        IPositionService positionService = new PositionService(mockPositionRepository.Object, new Logs("test.log", true));

        // Act
        var result = await positionService.DeletePositionAsync(d => d.PositionName == "CEO"); 

        // Assert
        Assert.True(result); // Expecting deletion to succeed

        
        mockPositionRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()), Times.Once);

    }
}
