namespace ReportApp.Models;

public class CheckInCheckOut
{
    public Guid Id { get; set; }    
    public DateTime ClientCheckedIn { get; set; }
    public DateTime ClientCheckedOut { get; set; }
}
