using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(PhoneNumber), IsUnique = true)]
public class EmployeePhoneNumberEntity
{

    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(30)")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(EmployeeEntity))]
    public int EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; } = null!;
}
