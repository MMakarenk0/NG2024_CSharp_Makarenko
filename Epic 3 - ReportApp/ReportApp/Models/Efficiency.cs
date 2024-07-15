namespace ReportApp.Models;

public class Efficiency
{
    public Guid Id { get;set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string Complains { get; set; }   
    public int SalesMade { get; set; }
    public Client Client { get; set; }
}
