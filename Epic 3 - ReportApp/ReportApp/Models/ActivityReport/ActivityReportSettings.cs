using ReportApp.Models.Entity;

namespace ReportApp.Models.ActivityReport;

public class ActivityReportSettings
{
    public string GeneratedFor { get; set; }
    public Client? GeneratedByClient { get; set; }
    public Admin? GeneratedByAdmin { get; set; }
}
