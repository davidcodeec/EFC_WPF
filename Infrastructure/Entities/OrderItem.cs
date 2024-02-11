using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[PrimaryKey("OrderItemId", "OrderId")]
public partial class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    [Key]
    public int OrderId { get; set; }

    [StringLength(200)]
    public string ArticleNumber { get; set; } = null!;

    public int ProductPriceId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    [ForeignKey("ArticleNumber")]
    [InverseProperty("OrderItems")]
    public virtual Product ArticleNumberNavigation { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductPriceId")]
    [InverseProperty("OrderItems")]
    public virtual ProductPrice ProductPrice { get; set; } = null!;
}
