using Microsoft.Extensions.DependencyInjection;
using ReportApp.Configuration;
using ReportApp.Services;
using ReportApp.Services.Activity;
using ReportApp.Services.CurrentShopItems;

var serviceCollection = new ServiceCollection();
ServiceConfigurator.ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();

var reportGenerators = serviceProvider.GetServices<IReportGenerator>();

foreach (var reportGenerator in reportGenerators)
{
    if (reportGenerator is ActivityReportGeneratorService)
    {
        reportGenerator.GenerateReport("./JsonExamples/ActivityReportAdminGenerated.json", "../../../Reports/ActivityReport.xlsx");
    }
    else if (reportGenerator is CurrentShopItemsReportGeneratorService)
    {
        reportGenerator.GenerateReport("./JsonExamples/CurrentShopItems2.json", "../../../Reports/CurrentShopItems.xlsx");
    }
}
