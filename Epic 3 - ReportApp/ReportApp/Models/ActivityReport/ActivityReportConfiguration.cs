namespace ReportApp.Models.ActivityReport;

public class ActivityReportConfiguration : ReportConfiguration
{
    public int HeaderKeyRow { get; set; }
    public int HeaderKeyColumn { get; set; }
    public int HeaderValueRow { get; set; }
    public int HeaderValueColumn { get; set; }
    public int HeaderRowsAmount { get; set; }
    public int GroupCount { get; set; }
}
