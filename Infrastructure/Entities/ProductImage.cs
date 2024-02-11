using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class ProductImage
{
    [Key]
    public int ProductImageId { get; set; }

    public string? ImagePath { get; set; }

    [InverseProperty("ProductImage")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
