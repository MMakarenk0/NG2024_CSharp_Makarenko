using ReportApp.Models;

namespace ReportApp.Services;

public class CheckInCheckOutService
{
    public CheckInCheckOut CalculateWorkHours(Client client, DateTime startTime, DateTime endTime)
    {
        return new CheckInCheckOut
        {
            ClientCheckedIn = startTime,
            ClientCheckedOut = endTime
        };
    }
}
