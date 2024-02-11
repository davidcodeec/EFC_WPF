using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Dtos;

namespace Infrastructure.Entities;

public class SalaryEntity
{
    [Key]
    public int SalaryId { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Now;

    public virtual ICollection<EmployeeEntity> Employees { get; set; } = new HashSet<EmployeeEntity>();

    // Convert from SalaryEntity to SalaryDto
    public static implicit operator SalaryDto(SalaryEntity? entity)
    {
        if (entity == null)
        {
            // Handle the case where 'entity' is null, for example, by returning a default SalaryDto
            return new SalaryDto
            {
                SalaryId = 0, // You may need to adjust the default values based on your requirements
                Amount = 0,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue
            };
        }

        // Your existing conversion logic
        return new SalaryDto
        {
            SalaryId = entity.SalaryId,
            Amount = entity.Amount,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        };
    }



    // Convert from SalaryDto to SalaryEntity
    public static implicit operator SalaryEntity(SalaryDto dto)
    {
        return new SalaryEntity
        {
            Amount = dto?.Amount ?? 0,
            StartDate = dto?.StartDate ?? DateTime.Now,
            EndDate = dto?.EndDate ?? DateTime.Now
        };
    }

    // Create SalaryEntity from SalaryDto with additional parameters
    public static SalaryEntity Create(SalaryDto salaryDto, DateTime startDate, DateTime endDate)
    {
        return new SalaryEntity
        {
            Amount = salaryDto?.Amount ?? 0,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    // Create IEnumerable<SalaryDto> from IEnumerable<SalaryEntity>
    public static IEnumerable<SalaryDto> Create(IEnumerable<SalaryEntity> salaryEntities)
    {
        return salaryEntities.Select(entity => (SalaryDto)entity).ToList();

    }
}
