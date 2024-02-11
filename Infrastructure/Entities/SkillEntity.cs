using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Dtos;

namespace Infrastructure.Entities;

public class SkillEntity
{
    [Key]
    public int SkillId { get; set; }


    [Column(TypeName = "nvarchar(50)")]
    public string SkillName { get; set; } = null!;

    public virtual ICollection<EmployeeEntity> Employees { get; set; } = new HashSet<EmployeeEntity>();

    // Implicit conversion operator from SkillEntity to SkillDto
    public static implicit operator SkillDto(SkillEntity skillEntity)
    {
        if (skillEntity == null)
            return null!;

        return new SkillDto
        {
            SkillId = skillEntity.SkillId,
            SkillName = skillEntity.SkillName
            // Map other properties...
        };
    }

    // Implicit conversion operator from SkillDto to SkillEntity
    public static implicit operator SkillEntity(SkillDto dto)
    {
        return new SkillEntity
        {
            SkillName = dto.SkillName ?? "Unknown",
        };
    }

    // Create SkillEntity from SkillDto with additional parameters if needed
    public static SkillEntity Create(SkillDto dto)
    {
        // Validation logic, if needed

        return new SkillEntity
        {
            SkillName = dto.SkillName ?? "Unknown",
        };
    }

    // Create IEnumerable<SkillDto> from IEnumerable<SkillEntity>
    public static IEnumerable<SkillDto> Create(IEnumerable<SkillEntity> entities)
    {
        return entities.Select(entity => (SkillDto)entity).ToList();
    }


}
