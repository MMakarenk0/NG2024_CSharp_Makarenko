namespace ReportApp.Interfaces
{
    public interface ICollectionReportSerializer<T>
    {
        List<T> DeserializeReportModels(string path);
    }
}
