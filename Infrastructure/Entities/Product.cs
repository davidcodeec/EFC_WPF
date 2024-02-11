using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class Product
{
    [Key]
    [StringLength(200)]
    public string ArticleNumber { get; set; } = null!;

    [StringLength(200)]
    public string ProductName { get; set; } = null!;

    [StringLength(200)]
    public string? Ingress { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    public int CategoryId { get; set; }

    public int SupplierId { get; set; }

    public int? ProductImageId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("ArticleNumberNavigation")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("ArticleNumberNavigation")]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    [ForeignKey("ProductImageId")]
    [InverseProperty("Products")]
    public virtual ProductImage? ProductImage { get; set; }

    [InverseProperty("ArticleNumberNavigation")]
    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier Supplier { get; set; } = null!;
}
