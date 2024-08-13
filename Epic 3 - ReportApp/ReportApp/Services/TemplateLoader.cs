using ClosedXML.Report;

namespace ReportApp.Services;

public class TemplateLoader
{
    public XLTemplate LoadTemplate(string path)
    {
        return new XLTemplate(path);
    }
}
