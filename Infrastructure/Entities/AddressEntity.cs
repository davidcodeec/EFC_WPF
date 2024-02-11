using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Infrastructure.Dtos;

namespace Infrastructure.Entities
{
    public class AddressEntity
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string StreetName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(20)")]
        public string? StreetNumber { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? PostalCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? City { get; set; }

        // Navigation property for EmployeeAddressEntity
        public virtual ICollection<EmployeeAddressEntity> EmployeeAddresses { get; set; } = new HashSet<EmployeeAddressEntity>();

        // Implicit conversion from AddressEntity to AddressDto
        public static implicit operator AddressDto(AddressEntity? entity)
        {
            return new AddressDto
            {
                StreetName = entity?.StreetName ?? string.Empty,
                StreetNumber = entity?.StreetNumber ?? string.Empty,
                PostalCode = entity?.PostalCode ?? string.Empty,
                City = entity?.City ?? string.Empty,
                // Other properties...
            };
        }

        // Static method to create AddressEntity from AddressDto
        public static AddressEntity Create(AddressDto dto)
        {
            return new AddressEntity
            {
                StreetName = dto.StreetName ?? string.Empty,
                StreetNumber = dto.StreetNumber,
                PostalCode = dto.PostalCode,
                City = dto.City,
                // Initialize other properties as needed...
            };
        }

        // Static method to create a list of AddressDto from a collection of AddressEntity
        public static IEnumerable<AddressDto> Create(IEnumerable<AddressEntity> entities)
        {
            return entities.Select(entity => (AddressDto)entity).ToList();
        }
    }
}
