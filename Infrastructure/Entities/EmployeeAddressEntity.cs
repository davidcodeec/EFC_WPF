using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(EmployeeId), nameof(AddressId), IsUnique = true)]
public class EmployeeAddressEntity
{
    [Key]
    public int EmployeeAddressId { get; set; }

    [Required]
    [ForeignKey(nameof(EmployeeEntity))]
    public int EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(AddressEntity))]
    public int AddressId { get; set; }
    public virtual AddressEntity Address { get; set; } = null!;
}

