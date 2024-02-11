using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class Address
{
    [Key]
    public int AddressId { get; set; }

    [StringLength(200)]
    public string StreetName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string StreetNumber { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string PostalCode { get; set; } = null!;

    [StringLength(50)]
    public string City { get; set; } = null!;

    [InverseProperty("Address")]
    public virtual ICollection<CustomersAddress> CustomersAddresses { get; set; } = new List<CustomersAddress>();
}
