using ClosedXML.Report;

namespace ReportApp.Services;

public abstract class ReportGeneratorBase
{
    protected readonly TemplateLoader templateLoader;
    protected readonly ConfigurationLoader configurationLoader;
    protected readonly ReportSerializer reportSerializer;

    protected XLTemplate template;

    protected ReportGeneratorBase(
        TemplateLoader TemplateLoader,
        ConfigurationLoader ConfigurationLoader,
        ReportSerializer ReportSerializer)
    {
        templateLoader = TemplateLoader;
        configurationLoader = ConfigurationLoader;
        reportSerializer = ReportSerializer;
    }

    public void GenerateReport(string outputPath)
    {
        FillSettings();
        FillHeader();
        FillReportData();
        Generate(outputPath);
    }

    // Settings can be optionally filled by the derived class
    protected virtual void FillSettings() { }

    // Header filling is also optional and depends on the report type
    protected virtual void FillHeader() { }

    // Abstract method to enforce filling report data in derived classes
    protected abstract void FillReportData();

    protected void Generate(string outputPath)
    {
        template.Generate();
        template.SaveAs(outputPath);
    }
}
