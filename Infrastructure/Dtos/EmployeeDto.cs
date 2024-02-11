using Infrastructure.Entities;

namespace Infrastructure.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public List<EmployeePhoneNumberEntity>? EmployeePhoneNumbers { get; set; }
    public List<EmployeeAddressEntity>? EmployeeAddresses { get; set; }
    public DateTime BirthDate { get; set; }
    public char Gender { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public int SalaryId { get; set; }
    public int SkillId { get; set; }
    public decimal Salary { get; set; }

    public string? DepartmentName { get; set; }
    public string? PositionName { get; set; } // Add this property
    public string? SkillName { get; set; }
    public decimal SalaryAmount { get; set; } // Add this property
    public string? Address { get; set; }
}
