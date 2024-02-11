namespace Infrastructure.Dtos;

public class UpdatedEmployeeDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public IEnumerable<string>? PhoneNumbers { get; set; }
    public AddressDto? Address { get; set; }
    public DateTime BirthDate { get; set; }
    public char Gender { get; set; }
    public SalaryDto? Salary { get; set; } // Use a single property to represent the updated salary amount
    public string? DepartmentName { get; set; }
    public string? PositionName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? SkillName { get; set; }
    public IEnumerable<AddressDto>? Addresses { get; set; }

    public int DepartmentId { get; set; }
    public int SalaryId { get; set; }
    public int SkillId { get; set; }
    public int PositionId { get; set; }
    public SkillDto? SkillDto { get; set; }
}
