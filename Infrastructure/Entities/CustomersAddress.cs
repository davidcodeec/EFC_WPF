using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("CustomerId", "AddressId", Name = "UQ__Customer__043FA676DA3DBAC3", IsUnique = true)]
public partial class CustomersAddress
{
    [Key]
    public int CustomerAddressId { get; set; }

    public int CustomerId { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("CustomersAddresses")]
    public virtual Address Address { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("CustomersAddresses")]
    public virtual Customer Customer { get; set; } = null!;
}
