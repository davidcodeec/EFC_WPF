using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace Infrastructure.Tests.Repositories;

public class EmployeeRepository_Tests
{
    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    
    private readonly Mock<ILogs> mockLogs = new();

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var mockRepository = new Mock<GenericRepository<EmployeeEntity>>(_employeeDbContext, mockLogs.Object);

        
        var employeeRepository = new EmployeeRepository(_employeeDbContext, mockLogs.Object);

        
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<EmployeeEntity>()))
            .ThrowsAsync(new Exception("Simulated exception"));

        // Act
        var result = await employeeRepository.CreateAsync(null!); 

        // Assert
        Assert.Null(result);

        
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_EmployeeEntities()
    {
        // Arrange
        var employeeEntities = new List<EmployeeEntity>
        {
            new EmployeeEntity
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                DepartmentId = 1,
                PositionId = 1,
                SalaryId = 1,
                SkillId = 1,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "123-456-7890" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street1", StreetNumber = "1", PostalCode = "12345", City = "City1" } } }
            },
            new EmployeeEntity
            {
                EmployeeId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DepartmentId = 2,
                PositionId = 2,
                SalaryId = 2,
                SkillId = 2,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "987-654-3210" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street2", StreetNumber = "2", PostalCode = "54321", City = "City2" } } }
            }
        };

        // Create a mock repository
        var mockRepository = new Mock<EmployeeRepository>(_employeeDbContext, mockLogs.Object);

        
        mockRepository.Setup(repo => repo.GetAllAsync())
                      .ReturnsAsync(employeeEntities);

        var employeeRepository = mockRepository.Object;

        // Act
        var result = await employeeRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeEntities.Count, result.Count()); 
        Assert.IsAssignableFrom<IEnumerable<EmployeeEntity>>(result); 
        foreach (var entity in employeeEntities)
        {
            Assert.Contains(result, e => e.EmployeeId == entity.EmployeeId &&
                                         e.FirstName == entity.FirstName &&
                                         e.LastName == entity.LastName &&
                                         e.DepartmentId == entity.DepartmentId &&
                                         e.PositionId == entity.PositionId &&
                                         e.SalaryId == entity.SalaryId &&
                                         e.SkillId == entity.SkillId &&
                                         e.EmployeePhoneNumbers.Any(p => p.PhoneNumber == entity.EmployeePhoneNumbers.First().PhoneNumber) &&
                                         e.EmployeeAddresses.Any(a => a.Address.StreetName == entity.EmployeeAddresses.First().Address.StreetName &&
                                                                      a.Address.StreetNumber == entity.EmployeeAddresses.First().Address.StreetNumber &&
                                                                      a.Address.PostalCode == entity.EmployeeAddresses.First().Address.PostalCode &&
                                                                      a.Address.City == entity.EmployeeAddresses.First().Address.City));
        }
    }




    [Fact]
    public async Task GetAsync_Should_Return_Filtered_EmployeeEntities()
    {
        // Arrange
        var mockRepository = new Mock<EmployeeRepository>(_employeeDbContext, mockLogs.Object);

       
        var employeeEntities = new List<EmployeeEntity>
        {
            new EmployeeEntity
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                DepartmentId = 1,
                PositionId = 1,
                SalaryId = 1,
                SkillId = 1,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "123-456-7890" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street1", StreetNumber = "1", PostalCode = "12345", City = "City1" } } }
            },
            new EmployeeEntity
            {
                EmployeeId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DepartmentId = 2,
                PositionId = 2,
                SalaryId = 2,
                SkillId = 2,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "987-654-3210" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street2", StreetNumber = "2", PostalCode = "54321", City = "City2" } } }
            }
        };

        
        mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<EmployeeEntity, bool>>>(), It.IsAny<int>()))
                      .ReturnsAsync((Expression<Func<EmployeeEntity, bool>> predicate, int take) =>
                      {
                          // Apply the predicate to filter the entities
                          var filteredEntities = employeeEntities.AsQueryable().Where(predicate).Take(take);
                          return filteredEntities.ToList();
                      });

        var repository = mockRepository.Object;

        // Act
        var result = await repository.GetAsync(d => d.EmployeeId == 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.IsAssignableFrom<IEnumerable<EmployeeEntity>>(result); 
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        // Arrange
        var employeeEntities = new List<EmployeeEntity>
        {
            new EmployeeEntity
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                DepartmentId = 1,
                PositionId = 1,
                SalaryId = 1,
                SkillId = 1,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "123-456-7890" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street1", StreetNumber = "1", PostalCode = "12345", City = "City1" } } }
            },
            new EmployeeEntity
            {
                EmployeeId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DepartmentId = 2,
                PositionId = 2,
                SalaryId = 2,
                SkillId = 2,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "987-654-3210" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street2", StreetNumber = "2", PostalCode = "54321", City = "City2" } } }
            }
        };

        
        var mockRepository = new Mock<EmployeeRepository>(_employeeDbContext, mockLogs.Object);

        _employeeDbContext.Employees.AddRange(employeeEntities);
        await _employeeDbContext.SaveChangesAsync();


        var employeeRepository = new EmployeeRepository(_employeeDbContext, mockLogs.Object);

        var employeeIdToFind = 1; 

        // Act
        var result = await employeeRepository.ExistsAsync(a => a.EmployeeId == employeeIdToFind);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public async Task ExistsAsync_Should_Return_False_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var employeeEntities = new List<EmployeeEntity>
        {
            new EmployeeEntity
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                DepartmentId = 1,
                PositionId = 1,
                SalaryId = 1,
                SkillId = 1,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "123-456-7890" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street1", StreetNumber = "1", PostalCode = "12345", City = "City1" } } }
            },
            new EmployeeEntity
            {
                EmployeeId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DepartmentId = 2,
                PositionId = 2,
                SalaryId = 2,
                SkillId = 2,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "987-654-3210" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street2", StreetNumber = "2", PostalCode = "54321", City = "City2" } } }
            }
        };

        
        var mockRepository = new Mock<EmployeeRepository>(_employeeDbContext, mockLogs.Object);

        _employeeDbContext.Employees.AddRange(employeeEntities);
        await _employeeDbContext.SaveChangesAsync();


        var employeeRepository = new EmployeeRepository(_employeeDbContext, mockLogs.Object);

        var employeeIdToFind = 3;

        // Act
        var result = await employeeRepository.ExistsAsync(a => a.EmployeeId == employeeIdToFind);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task UpdateAsync_Should_Update_EmployeeEntities()
    {
        // Arrange
        var employeeEntities = new List<EmployeeEntity>
    {
        new EmployeeEntity
        {
            EmployeeId = 1,
            FirstName = "John",
            LastName = "Doe",
            DepartmentId = 1,
            PositionId = 1,
            SalaryId = 1,
            SkillId = 1,
            EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "123-456-7890" } },
            EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street1", StreetNumber = "1", PostalCode = "12345", City = "City1" } } }
        },
        new EmployeeEntity
        {
            EmployeeId = 2,
            FirstName = "Jane",
            LastName = "Smith",
            DepartmentId = 2,
            PositionId = 2,
            SalaryId = 2,
            SkillId = 2,
            EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "987-654-3210" } },
            EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street2", StreetNumber = "2", PostalCode = "54321", City = "City2" } } }
        }
    };

        var mockRepository = new Mock<EmployeeRepository>(_employeeDbContext, mockLogs.Object);
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<EmployeeEntity, bool>>>(), It.IsAny<EmployeeEntity>()))
                      .ReturnsAsync((Expression<Func<EmployeeEntity, bool>> predicate, EmployeeEntity updatedEntity) =>
                      {
                          
                          return updatedEntity;
                      });

        var employeeRepository = mockRepository.Object;

        var updatedEntity = new EmployeeEntity { EmployeeId = 1, FirstName = "Joe", LastName = "John" };

        // Act
        var result = await employeeRepository.UpdateAsync(
            d => d.EmployeeId == 1, 
            updatedEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedEntity.EmployeeId, result.EmployeeId);
        Assert.Equal(updatedEntity.FirstName, result.FirstName); 
        Assert.Equal(updatedEntity.LastName, result.LastName); 
    }


    [Fact]
    public async Task DeleteAsync_Should_Delete_EmployeeEntities()
    {
        var employeeEntities = new List<EmployeeEntity>
        {
            new EmployeeEntity
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                DepartmentId = 1,
                PositionId = 1,
                SalaryId = 1,
                SkillId = 1,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "123-456-7890" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street1", StreetNumber = "1", PostalCode = "12345", City = "City1" } } }
            },
            new EmployeeEntity
            {
                EmployeeId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DepartmentId = 2,
                PositionId = 2,
                SalaryId = 2,
                SkillId = 2,
                EmployeePhoneNumbers = new List<EmployeePhoneNumberEntity> { new EmployeePhoneNumberEntity { PhoneNumber = "987-654-3210" } },
                EmployeeAddresses = new List<EmployeeAddressEntity> { new EmployeeAddressEntity { Address = new AddressEntity { StreetName = "Street2", StreetNumber = "2", PostalCode = "54321", City = "City2" } } }
            }
        };

       
        var mockRepository = new Mock<EmployeeRepository>(_employeeDbContext, mockLogs.Object);

        _employeeDbContext.Employees.AddRange(employeeEntities);
        await _employeeDbContext.SaveChangesAsync();

        var employeeRepository = new EmployeeRepository(_employeeDbContext, mockLogs.Object);

        // Act
        bool result = false;
        try
        {
            result = await employeeRepository.DeleteAsync(d => d.EmployeeId == 2);
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

        
        var deletedEntity = await _employeeDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == 2);
        Assert.Null(deletedEntity);
    }
}
