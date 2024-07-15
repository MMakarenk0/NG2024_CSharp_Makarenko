using ReportApp.Models.Entity;

namespace ReportApp.Models.ActivityReport;

public class ActivityReportModel
{
    public Guid Id { get; set; }
    public Client? GeneratedByClient { get; set; }
    public Admin? GeneratedByAdmin { get; set; }
    public DateTime WorkdayStartTime { get; set; }
    public DateTime WorkdayEndTime { get; set; }
    public string Office { get; set; }
    public Client ReportGeneratedFor { get; set; }
    public List<Complains>? Complains { get; set; }
}
