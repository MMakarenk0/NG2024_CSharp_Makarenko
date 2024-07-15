namespace ReportApp.Services;

public interface IReportSerializer<T>
{
    T DeserializeReportModel(string path);
}
