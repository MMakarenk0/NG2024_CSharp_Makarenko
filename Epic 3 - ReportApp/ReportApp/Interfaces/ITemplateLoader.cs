using ClosedXML.Report;

namespace ReportApp.Services;

public interface ITemplateLoader
{
    XLTemplate LoadTemplate(string path);
}

