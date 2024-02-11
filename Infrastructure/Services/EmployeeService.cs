using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories.Employee;
using Infrastructure.Utils;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class EmployeeService(
    IEmployeeRepository employeeRepository,
    IDepartmentService departmentService,
    IPositionService positionService,
    ISkillService skillService,
    IAddressService addressService,
    ISalaryService salaryService,
    ILogs logs) : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IDepartmentService _departmentService = departmentService;
    private readonly IPositionService _positionService = positionService;
    private readonly ISkillService _skillService = skillService;
    private readonly IAddressService _addressService = addressService;
    private readonly ISalaryService _salaryService = salaryService;
    private readonly ILogs _logs = logs;



    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Expression<Func<EmployeeEntity, bool>> predicate, int take = -1)
    {
        try
        {
            var employeeEntities = await _employeeRepository.GetAsync(predicate, take);
            return EmployeeEntity.Create(employeeEntities);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeService - GetEmployeesAsync");
            return Enumerable.Empty<EmployeeDto>();
        }
    }

    public async Task<EmployeeDto?> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        try
        {
            // Retrieve department, position, skill, and salary data
            var departmentDto = await _departmentService.GetOneDepartmentAsync(x => x.DepartmentName == createEmployeeDto.DepartmentName);
            var positionDto = await _positionService.GetOnePositionAsync(x => x.PositionName == createEmployeeDto.PositionName);
            var skillDto = await _skillService.GetOneSkillAsync(x => x.SkillName == createEmployeeDto.SkillName);
            var salaryDto = await _salaryService.GetOneSalaryAsync(x => x.Amount == createEmployeeDto.SalaryAmount.Amount);

            // Check if all required data is found
            if (departmentDto != null && positionDto != null && skillDto != null && salaryDto != null)
            {
                // Create new employee entity
                var employeeEntity = new EmployeeEntity
                {
                    FirstName = createEmployeeDto.FirstName,
                    LastName = createEmployeeDto.LastName,
                    Email = createEmployeeDto.Email,
                    BirthDate = createEmployeeDto.BirthDate,
                    Gender = createEmployeeDto.Gender,
                    DepartmentId = departmentDto.DepartmentId,
                    PositionId = positionDto.PositionId,
                    SalaryId = salaryDto.SalaryId,
                    SkillId = skillDto.SkillId
                };

                // Save the employeeEntity to the database
                await _employeeRepository.CreateAsync(employeeEntity);

                // Convert EmployeeEntity to EmployeeDto before returning
                return new EmployeeDto
                {
                    Id = employeeEntity.EmployeeId,
                    FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName,
                    Address = employeeEntity.EmployeeAddresses.FirstOrDefault()?.Address?.ToString(),
                    Salary = salaryDto.Amount ?? 0, // Handle null by providing a default value
                    SkillName = skillDto.SkillName,
                    DepartmentName = departmentDto.DepartmentName
                };
            }
            else
            {
                // Department, position, skill, or salary not found
                return null;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeService - CreateEmployeeAsync");
            return null;
        }
    }


    public async Task<EmployeeDto?> GetOneEmployeeAsync(Expression<Func<EmployeeEntity, bool>> predicate)
    {
        try
        {
            var employeeEntity = await _employeeRepository.GetOneAsync(predicate);

            if (employeeEntity != null)
            {
                var departmentDto = await _departmentService.GetOneDepartmentAsync(d => d.DepartmentId == employeeEntity.DepartmentId);
                var positionDto = await _positionService.GetOnePositionAsync(p => p.PositionId == employeeEntity.PositionId);
                var skillDto = await _skillService.GetOneSkillAsync(s => s.SkillId == employeeEntity.SkillId);
                var salaryDto = await _salaryService.GetOneSalaryAsync(s => s.SalaryId == employeeEntity.SalaryId);

                // Assuming an employee can have multiple addresses and you want to display the first address
                var firstAddressEntity = employeeEntity.EmployeeAddresses.FirstOrDefault()?.Address;

                var employeeDto = new EmployeeDto
                {
                    Id = employeeEntity.EmployeeId,
                    FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName,
                    BirthDate = employeeEntity.BirthDate,
                    Gender = employeeEntity.Gender,
                    DepartmentId = employeeEntity.DepartmentId,
                    PositionId = employeeEntity.PositionId,
                    SalaryId = employeeEntity.SalaryId,
                    SkillId = employeeEntity.SkillId,
                    Address = firstAddressEntity?.ToString(), 
                    Salary = salaryDto?.Amount ?? 0,
                    SkillName = skillDto?.SkillName,
                    DepartmentName = departmentDto?.DepartmentName
                };

                return employeeDto;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeService - GetOneEmployeeAsync");
        }

        return null;
    }




    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeeAsync()
    {
        try
        {
            var employeeEntities = await _employeeRepository.GetAllAsync();
            if (employeeEntities != null)
            {
                var employeeDtos = new List<EmployeeDto>();

                foreach (var employeeEntity in employeeEntities)
                {
                    var departmentDto = await _departmentService.GetOneDepartmentAsync(d => d.DepartmentId == employeeEntity.DepartmentId);
                    var positionDto = await _positionService.GetOnePositionAsync(p => p.PositionId == employeeEntity.PositionId);
                    var skillDto = await _skillService.GetOneSkillAsync(s => s.SkillId == employeeEntity.SkillId);
                    var salaryDto = await _salaryService.GetOneSalaryAsync(s => s.SalaryId == employeeEntity.SalaryId);
                    var firstAddressEntity = employeeEntity.EmployeeAddresses.FirstOrDefault();

                    var employeeDto = new EmployeeDto
                    {
                        Id = employeeEntity.EmployeeId,
                        FirstName = employeeEntity.FirstName,
                        LastName = employeeEntity.LastName,
                        BirthDate = employeeEntity.BirthDate,
                        Gender = employeeEntity.Gender,
                        DepartmentId = employeeEntity.DepartmentId,
                        PositionId = employeeEntity.PositionId,
                        SalaryId = employeeEntity.SalaryId,
                        SkillId = employeeEntity.SkillId,
                        Address = firstAddressEntity?.ToString(),
                        Salary = salaryDto?.Amount ?? 0,
                        SkillName = skillDto?.SkillName,
                        DepartmentName = departmentDto?.DepartmentName
                    };

                    employeeDtos.Add(employeeDto);
                }

                return employeeDtos;
            }
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeService - GetAllEmployeesAsync");
        }

        return Enumerable.Empty<EmployeeDto>();
    }



    public async Task<EmployeeDto> UpdateEmployeeAsync(Expression<Func<EmployeeEntity, bool>> predicate, UpdatedEmployeeDto updatedEmployeeDto)
    {
        try
        {
            // Retrieve the employee entity to update
            var existingEmployeeEntity = await _employeeRepository.GetOneAsync(predicate);

            if (existingEmployeeEntity != null)
            {
                // Update the properties of the existing entity
                existingEmployeeEntity.FirstName = updatedEmployeeDto.FirstName;
                existingEmployeeEntity.LastName = updatedEmployeeDto.LastName;
                existingEmployeeEntity.Email = updatedEmployeeDto.Email;
                existingEmployeeEntity.BirthDate = updatedEmployeeDto.BirthDate;
                existingEmployeeEntity.Gender = updatedEmployeeDto.Gender; // Assign char value directly
                existingEmployeeEntity.DepartmentId = updatedEmployeeDto.DepartmentId;
                existingEmployeeEntity.PositionId = updatedEmployeeDto.PositionId;

                // Save changes to the database
                var updatedEntity = await _employeeRepository.UpdateAsync(predicate, existingEmployeeEntity);

                // Convert the updated entity back to DTO using the implicit operator
                return updatedEntity;
            }
            else
            {
                // If the employee entity to update is not found, throw an exception or return null
                throw new Exception("Employee entity not found.");
            }
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow it
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeService - UpdateEmployeeAsync");
            throw; // Rethrow the exception to let the caller handle it
        }
    }






    public async Task<bool> DeleteEmployeeAsync(Expression<Func<EmployeeEntity, bool>> predicate)
    {
        try
        {
            return await _employeeRepository.DeleteAsync(predicate);
        }
        catch (Exception ex)
        {
            await _logs.LogToFileAsync(ex.ToString(), "EmployeeService - DeleteEmployeeAsync");
            return false;
        }
    }

}
