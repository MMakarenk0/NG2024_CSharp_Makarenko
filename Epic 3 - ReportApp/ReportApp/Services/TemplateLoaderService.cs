using ClosedXML.Report;

namespace ReportApp.Services;

public class TemplateLoaderService : ITemplateLoader
{
    public XLTemplate LoadTemplate(string path)
    {
        return new XLTemplate(path);
    }
}
