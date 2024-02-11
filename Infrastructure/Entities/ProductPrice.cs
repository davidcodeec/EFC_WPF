using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class ProductPrice
{
    [Key]
    public int ProductPriceId { get; set; }

    [StringLength(200)]
    public string ArticleNumber { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "money")]
    public decimal? DiscountPrice { get; set; }

    [ForeignKey("ArticleNumber")]
    [InverseProperty("ProductPrices")]
    public virtual Product ArticleNumberNavigation { get; set; } = null!;

    [InverseProperty("ProductPrice")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
