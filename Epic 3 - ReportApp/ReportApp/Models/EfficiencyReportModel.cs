namespace ReportApp.Models;

public class EfficiencyReportModel
{
    public Guid Id { get; set; }

    //We are calculating Efficiency for this Client
    public Client Client { get; set; }

    //We are calculating Efficiency for this Admin
    public Admin Admin { get; set; }

    public Efficiency Efficiency { get; set; }
    public decimal SalaryInCurrentMonth { get; set; }
    public decimal BonusInCurrentMonth { get; set; }
    public Admin GeneratedBy { get; set; }
}
