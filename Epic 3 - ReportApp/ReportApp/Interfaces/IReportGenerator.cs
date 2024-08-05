namespace ReportApp.Services;

public interface IReportGenerator
{
    void GenerateReport(string jsonPath, string outputPath);
}