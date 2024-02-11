using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Dtos;

namespace Infrastructure.Entities
{
    public class DepartmentEntity
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string DepartmentName { get; set; } = null!;

        public virtual ICollection<EmployeeEntity> Employees { get; set; } = new HashSet<EmployeeEntity>();

        // Implicit conversion operator from DepartmentEntity to DepartmentDto
        public static implicit operator DepartmentDto(DepartmentEntity entity)
        {
            if (entity == null)
                return null!;

            return new DepartmentDto
            {
                DepartmentId = entity.DepartmentId,
                DepartmentName = entity.DepartmentName ?? "Unknown",
            };
        }

        // Implicit conversion operator from DepartmentDto to DepartmentEntity
        public static implicit operator DepartmentEntity(DepartmentDto dto)
        {
            return new DepartmentEntity
            {
                DepartmentName = dto?.DepartmentName ?? "Unknown",
            };
        }

        // Create DepartmentEntity from DepartmentDto with additional parameters if needed
        public static DepartmentEntity Create(DepartmentDto dto)
        {
            return new DepartmentEntity
            {
                DepartmentName = dto?.DepartmentName ?? "Unknown",
            };
        }

        // Create IEnumerable<DepartmentDto> from IEnumerable<DepartmentEntity>
        public static IEnumerable<DepartmentDto> Create(IEnumerable<DepartmentEntity> entities)
        {
            return entities.Select(entity => (DepartmentDto)entity).ToList();
        }
    }
}
