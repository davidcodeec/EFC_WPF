namespace Infrastructure.Dtos;

public class UpdatedAddressDto
{
    public int AddressId { get; set; }
    public string? StreetName { get; set; }
    public string? StreetNumber { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}
