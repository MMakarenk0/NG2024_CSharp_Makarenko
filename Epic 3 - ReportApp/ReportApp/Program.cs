using Microsoft.Extensions.DependencyInjection;
using ReportApp.Configuration;
using ReportApp.Services.ActivityReport;
using ReportApp.Services.CurrentItemsShop;

var serviceCollection = new ServiceCollection();
ServiceConfigurator.ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();

var ActivityReportGenerator = serviceProvider.GetService<ActivityReportGeneratorService>();
var CurrentShopItemsReportGenerator = serviceProvider.GetService<CurrentShopItemsReportGeneratorService>();

ActivityReportGenerator.GenerateReport("../../../Reports/ActivityReport.xlsx");
CurrentShopItemsReportGenerator.GenerateReport("../../../Reports/CurrentShopItems.xlsx");
