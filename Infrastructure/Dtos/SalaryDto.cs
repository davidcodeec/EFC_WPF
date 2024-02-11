namespace Infrastructure.Dtos;

public class SalaryDto
{
    public int SalaryId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
