using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("ArticleNumber", "CategoryId", Name = "UQ__ProductC__9D0982E20ABAA23C", IsUnique = true)]
public partial class ProductCategory
{
    [Key]
    public int ProductCategoryId { get; set; }

    [StringLength(200)]
    public string ArticleNumber { get; set; } = null!;

    public int CategoryId { get; set; }

    [ForeignKey("ArticleNumber")]
    [InverseProperty("ProductCategories")]
    public virtual Product ArticleNumberNavigation { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("ProductCategories")]
    public virtual Category Category { get; set; } = null!;
}
