using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Services;
using Infrastructure.Utils;
using Moq;
using System.Linq.Expressions;

namespace Infrastructure.Tests.Services;

public class SalaryService_Tests
{

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var salaryEntity = new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) };

        var mockRepository = new Mock<ISalaryRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<SalaryEntity>()))
                        .ThrowsAsync(new Exception("Simulated exception"));

        var salaryRepository = mockRepository.Object;

        ISalaryService salaryService = new SalaryService(salaryRepository, mockLogs.Object);

        // Act
        var result = await salaryService.CreateSalaryAsync(salaryEntity.Amount, salaryEntity.StartDate, salaryEntity.EndDate);

        // Assert
        Assert.Null(result);

       
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_All_SalaryDtos()
    {
        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        var salaryDtos = salaryEntities.Select(entity => new SalaryDto { SalaryId = entity.SalaryId });

       
        var mockRepository = new Mock<ISalaryRepository>();

        
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(salaryEntities);

        var salaryRepository = mockRepository.Object;

        ISalaryService salaryService = new SalaryService(salaryRepository, mockLogs.Object);

        // Act
        var result = await salaryService.GetAllSalaryAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.All(dto => dto is SalaryDto));

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

        var mocksalaryRepository = new Mock<ISalaryRepository>();

        mocksalaryRepository.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<SalaryEntity, bool>>>(),
            It.IsAny<int>()))
            .ReturnsAsync((Expression<Func<SalaryEntity, bool>> predicate, int take) =>
            {
                
                var filteredEntities = salaryEntities.Where(predicate.Compile()).ToList();
               
                if (take > 0)
                {
                    filteredEntities = filteredEntities.Take(take).ToList();
                }
                return filteredEntities;
            });

        
        ISalaryService salaryService = new SalaryService(mocksalaryRepository.Object, new Logs("test.log", true));

        // Act
        var result = await salaryService.GetSalariesAsync(d => d.SalaryId == 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count()); 
        Assert.All(result, skill => Assert.Equal(1, skill.SalaryId));
    }



    [Fact]
    public async Task GetOneAsync_Should_Return_One_SalaryEntity()
    {
        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        var mockSalaryRepository = new Mock<ISalaryRepository>();

        
        mockSalaryRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>()))
                                .ReturnsAsync((Expression<Func<SalaryEntity, bool>> predicate) =>
                                {
                                    return salaryEntities.FirstOrDefault(predicate.Compile());
                                });

        // Act
        
        ISalaryService salaryService = new SalaryService(mockSalaryRepository.Object, new Logs("test.log", true));
        var result = await salaryService.GetOneSalaryAsync(d => d.SalaryId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.SalaryId);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_SalaryEntity()
    {
        // Arrange
        var updatedSalaryDto = new UpdatedSalaryDto
        {
            SalaryId = 1,
            Amount = 27000,
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate = DateTime.UtcNow.AddDays(30)
        };

        var existingSalaryEntity = new SalaryEntity
        {
            SalaryId = updatedSalaryDto.SalaryId,
            Amount = 25000,
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate = DateTime.UtcNow.AddDays(30)
        };

        var mockSalaryRepository = new Mock<ISalaryRepository>();
        mockSalaryRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>()))
                            .ReturnsAsync(existingSalaryEntity);

        mockSalaryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>(), It.IsAny<SalaryEntity>()))
                            .ReturnsAsync((Expression<Func<SalaryEntity, bool>> predicate, SalaryEntity updatedEntity) =>
                            {
                                
                                return updatedEntity;
                            });

        var salaryService = new SalaryService(mockSalaryRepository.Object, new Logs("test.log", true));

        // Act
        var result = await salaryService.UpdateSalaryAsync(updatedSalaryDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedSalaryDto.SalaryId, result.SalaryId); 
        Assert.Equal(updatedSalaryDto.Amount, result.Amount); 
    }


    [Fact]
    public async Task DeleteAsync_Should_Delete_SalaryEntity()
    {
        // Arrange
        var salaryEntities = new List<SalaryEntity>
        {
            new SalaryEntity { SalaryId = 1, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30) },
            new SalaryEntity { SalaryId = 2, Amount = 25000, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(45) }
        };

        var mockSalaryRepository = new Mock<ISalaryRepository>();
        mockSalaryRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>()))
                                .ReturnsAsync(true); 

        ISalaryService salaryService = new SalaryService(mockSalaryRepository.Object, new Logs("test.log", true));

        // Act
        var result = await salaryService.DeleteSalaryAsync(d => d.SalaryId == 1); 

        // Assert
        Assert.True(result); // Expecting deletion to succeed

        
        mockSalaryRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>()), Times.Once);
    }



}
