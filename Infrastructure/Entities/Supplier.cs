using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [StringLength(50)]
    public string SupplierNumber { get; set; } = null!;

    [StringLength(50)]
    public string ContactPerson { get; set; } = null!;

    [StringLength(50)]
    public string SupplierEmail { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string SupplierPhone { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
