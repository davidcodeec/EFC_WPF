using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[PrimaryKey("CustomerId", "PhoneNumber")]
public partial class CustomersPhoneNumber
{
    [Key]
    public int CustomerId { get; set; }

    [Key]
    [StringLength(30)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;
}
