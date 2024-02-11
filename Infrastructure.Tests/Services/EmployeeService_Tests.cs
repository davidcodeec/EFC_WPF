using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Services;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Tests.Services;

public class EmployeeService_Tests
{
    
    private readonly Mock<ILogs> mockLogs = new();

    private readonly EmployeeDbContext _employeeDbContext = new(new DbContextOptionsBuilder<EmployeeDbContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task CreateAsync_Should_Handle_Exception_And_Return_Null()
    {
        // Arrange
        var createEmployeeDto = new CreateEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            BirthDate = new DateTime(1990, 5, 15),
            Gender = 'M',
            DepartmentName = "Engineering",
            PositionName = "Software Engineer",
            SkillName = "C# Programming",
            SalaryAmount = new SalaryDto { Amount = 20000 },
            Address = new AddressDto
            {
                AddressId = 1,
                City = "Stockholm",
                PostalCode = "11234",
                StreetName = "Sveavägen",
                StreetNumber = "45A",
            }
        };

        var mockDepartmentService = new Mock<IDepartmentService>();
        mockDepartmentService.Setup(service => service.GetOneDepartmentAsync(It.IsAny<Expression<Func<DepartmentEntity, bool>>>()))
                             .ReturnsAsync(new DepartmentDto { DepartmentId = 1, DepartmentName = "Department 1" });

        var mockPositionService = new Mock<IPositionService>();
        mockPositionService.Setup(service => service.GetOnePositionAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()))
                           .ReturnsAsync(new PositionDto { PositionId = 1, PositionName = "Position 1" });

        var mockSkillService = new Mock<ISkillService>();
        mockSkillService.Setup(service => service.GetOneSkillAsync(It.IsAny<Expression<Func<SkillEntity, bool>>>()))
                        .ReturnsAsync(new SkillDto { SkillId = 1, SkillName = "Skill 1" });

        var mockAddressService = new Mock<IAddressService>();
        mockAddressService.Setup(service => service.GetOneAddressAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                         .ReturnsAsync(new AddressDto { AddressId = 1, StreetNumber = "45A", PostalCode = "11234", StreetName = "Sveavägen", City = "Stockholm" });

        var mockSalaryService = new Mock<ISalaryService>();
        mockSalaryService.Setup(service => service.GetOneSalaryAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>()))
                         .ReturnsAsync(new SalaryDto { SalaryId = 1, Amount = 1000 });

        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        mockEmployeeRepository.Setup(repo => repo.CreateAsync(It.IsAny<EmployeeEntity>()))
                              .ThrowsAsync(new Exception("Simulated exception"));

        IEmployeeService employeeService = new EmployeeService(
        mockEmployeeRepository.Object,
        mockDepartmentService.Object,
        mockPositionService.Object,
        mockSkillService.Object,
        mockAddressService.Object,
        mockSalaryService.Object,
        mockLogs.Object);


        // Act
        var result = await employeeService.CreateEmployeeAsync(createEmployeeDto);

        // Assert
        Assert.Null(result);

       
        mockLogs.Verify(logs => logs.LogToFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }



    [Fact]
    public async Task GetAllAsync_Should_Return_All_EmployeeDtos()
    {
        // Arrange
        var createEmployeeDto = new CreateEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            BirthDate = new DateTime(1990, 5, 15),
            Gender = 'M',
            DepartmentName = "Engineering",
            PositionName = "Software Engineer",
            SkillName = "C# Programming",
            SalaryAmount = new SalaryDto { Amount = 20000 },
            Address = new AddressDto
            {
                AddressId = 1,
                City = "Stockholm",
                PostalCode = "11234",
                StreetName = "Sveavägen",
                StreetNumber = "45A",
            }
        };

        var mockDepartmentService = new Mock<IDepartmentService>();
        mockDepartmentService.Setup(service => service.GetOneDepartmentAsync(It.IsAny<Expression<Func<DepartmentEntity, bool>>>()))
                             .ReturnsAsync(new DepartmentDto { DepartmentId = 1, DepartmentName = "Engineering" });

        var mockPositionService = new Mock<IPositionService>();
        mockPositionService.Setup(service => service.GetOnePositionAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()))
                           .ReturnsAsync(new PositionDto { PositionId = 1, PositionName = "Software Engineer" });

        var mockSkillService = new Mock<ISkillService>();
        mockSkillService.Setup(service => service.GetOneSkillAsync(It.IsAny<Expression<Func<SkillEntity, bool>>>()))
                        .ReturnsAsync(new SkillDto { SkillId = 1, SkillName = "C# Programming" });

        var mockAddressService = new Mock<IAddressService>();
        mockAddressService.Setup(service => service.GetOneAddressAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                         .ReturnsAsync(new AddressDto { AddressId = 1, City = "Stockholm", PostalCode = "11234", StreetName = "Sveavägen", StreetNumber = "45A" });

        var mockSalaryService = new Mock<ISalaryService>();
        mockSalaryService.Setup(service => service.GetOneSalaryAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>()))
                         .ReturnsAsync(new SalaryDto { SalaryId = 1, Amount = 20000 });

        var mockEmployeeRepository = new Mock<IEmployeeRepository>();

        
        var employeeEntities = new List<EmployeeEntity>
        {
            new EmployeeEntity
            {
                FirstName = createEmployeeDto.FirstName,
                LastName = createEmployeeDto.LastName,
                Email = createEmployeeDto.Email,
                BirthDate = createEmployeeDto.BirthDate,
                Gender = createEmployeeDto.Gender,
                DepartmentId = 1, 
                PositionId = 1, 
                SalaryId = 1, 
                SkillId = 1, 
                EmployeeAddresses = new List<EmployeeAddressEntity> 
                {
                    new EmployeeAddressEntity { AddressId = 1 } 
                }
            }
        };


        
        mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                              .ReturnsAsync(employeeEntities);

        var employeeRepository = mockEmployeeRepository.Object;

        var mockLogs = new Mock<ILogs>();

        IEmployeeService employeeService = new EmployeeService(
            employeeRepository,
            mockDepartmentService.Object,
            mockPositionService.Object,
            mockSkillService.Object,
            mockAddressService.Object,
            mockSalaryService.Object,
            mockLogs.Object);

        // Act
        var result = await employeeService.GetAllEmployeeAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeEntities.Count, result.Count());

        
        Assert.True(result.All(dto => dto is EmployeeDto));
    }


    [Fact]
    public async Task GetAsync_Should_Return_Filtered_EmployeeEntities()
    {
        // Arrange
        var createEmployeeDto = new CreateEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            BirthDate = new DateTime(1990, 5, 15),
            Gender = 'M',
            DepartmentName = "Engineering",
            PositionName = "Software Engineer",
            SkillName = "C# Programming",
            SalaryAmount = new SalaryDto { Amount = 20000 },
            Address = new AddressDto
            {
                AddressId = 1,
                City = "Stockholm",
                PostalCode = "11234",
                StreetName = "Sveavägen",
                StreetNumber = "45A",
            }
        };

        var mockDepartmentService = new Mock<IDepartmentService>();
        mockDepartmentService.Setup(service => service.GetOneDepartmentAsync(It.IsAny<Expression<Func<DepartmentEntity, bool>>>()))
                             .ReturnsAsync(new DepartmentDto { DepartmentId = 1, DepartmentName = "Engineering" });

        var mockPositionService = new Mock<IPositionService>();
        mockPositionService.Setup(service => service.GetOnePositionAsync(It.IsAny<Expression<Func<PositionEntity, bool>>>()))
                           .ReturnsAsync(new PositionDto { PositionId = 1, PositionName = "Software Engineer" });

        var mockSkillService = new Mock<ISkillService>();
        mockSkillService.Setup(service => service.GetOneSkillAsync(It.IsAny<Expression<Func<SkillEntity, bool>>>()))
                        .ReturnsAsync(new SkillDto { SkillId = 1, SkillName = "C# Programming" });

        var mockAddressService = new Mock<IAddressService>();
        mockAddressService.Setup(service => service.GetOneAddressAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>()))
                         .ReturnsAsync(new AddressDto { AddressId = 1, City = "Stockholm", PostalCode = "11234", StreetName = "Sveavägen", StreetNumber = "45A" });

        var mockSalaryService = new Mock<ISalaryService>();
        mockSalaryService.Setup(service => service.GetOneSalaryAsync(It.IsAny<Expression<Func<SalaryEntity, bool>>>()))
                         .ReturnsAsync(new SalaryDto { SalaryId = 1, Amount = 20000 });

        var mockEmployeeRepository = new Mock<IEmployeeRepository>();

        mockEmployeeRepository.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<EmployeeEntity, bool>>>(),
            It.IsAny<int>()))
            .ReturnsAsync((Expression<Func<EmployeeEntity, bool>> predicate, int take) =>
            {
                
                return new List<EmployeeEntity>();
            });

        var employeeRepository = mockEmployeeRepository.Object;

        
        IEmployeeService employeeService = new EmployeeService(
            employeeRepository,
            mockDepartmentService.Object,
            mockPositionService.Object,
            mockSkillService.Object,
            mockAddressService.Object,
            mockSalaryService.Object,
            null); 

        // Act
        var result = await employeeService.GetEmployeesAsync(e => e.EmployeeId == 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); 
    }



    [Fact]
    public async Task GetOneAsync_Should_Return_One_EmployeeEntity()
    {
        // Arrange
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        mockEmployeeRepository.Setup(repo => repo.GetOneAsync(It.IsAny<Expression<Func<EmployeeEntity, bool>>>()))
                              .ReturnsAsync(new EmployeeEntity { EmployeeId = 1, SalaryId = 1 });

        var employeeRepository = mockEmployeeRepository.Object;

        var mockDepartmentService = new Mock<IDepartmentService>();
        var mockPositionService = new Mock<IPositionService>();
        var mockSkillService = new Mock<ISkillService>();
        var mockAddressService = new Mock<IAddressService>();
        var mockSalaryService = new Mock<ISalaryService>();

        
        IEmployeeService employeeService = new EmployeeService(
            employeeRepository,
            mockDepartmentService.Object,
            mockPositionService.Object,
            mockSkillService.Object,
            mockAddressService.Object,
            mockSalaryService.Object,
            null); 

        // Act
        var result = await employeeService.GetOneEmployeeAsync(e => e.SalaryId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.SalaryId);
    }

}
