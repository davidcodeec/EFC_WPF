using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Dtos;

namespace Infrastructure.Entities
{
    public class PositionEntity
    {
        [Key]
        public int PositionId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string PositionName { get; set; } = null!;

        public virtual ICollection<EmployeeEntity> Employees { get; set; } = new HashSet<EmployeeEntity>();

        // Implicit conversion operator from PositionEntity to PositionDto
        public static implicit operator PositionDto(PositionEntity entity)
        {
            if (entity == null)
                return null!;

            return new PositionDto
            {
                PositionId = entity.PositionId,
                PositionName = entity.PositionName,
            };
        }

        // Implicit conversion operator from PositionDto to PositionEntity
        public static implicit operator PositionEntity(PositionDto dto)
        {
            return new PositionEntity
            {
                PositionName = dto.PositionName ?? "Unknown",
            };
        }

        // Create PositionEntity from PositionDto with additional parameters if needed
        public static PositionEntity Create(PositionDto dto)
        {
            return new PositionEntity
            {
                PositionName = dto.PositionName ?? "Unknown",
            };
        }

        // Create IEnumerable<PositionDto> from IEnumerable<PositionEntity>
        public static IEnumerable<PositionDto> Create(IEnumerable<PositionEntity> entities)
        {
            return entities.Select(entity => (PositionDto)entity).ToList();
        }
    }
}
