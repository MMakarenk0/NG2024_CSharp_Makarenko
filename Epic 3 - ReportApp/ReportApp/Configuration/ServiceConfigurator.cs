using Microsoft.Extensions.DependencyInjection;
using ReportApp.Interfaces;
using ReportApp.Models.ActivityReport;
using ReportApp.Models.CurrentShopItems;
using ReportApp.Services;
using ReportApp.Services.Activity;
using ReportApp.Services.CurrentShopItems;

namespace ReportApp.Configuration
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITemplateLoader, TemplateLoaderService>();

            // ActivityReport Services
            services.AddScoped<IConfigurationLoader<ActivityReportConfiguration>, ActivityConfigurationLoaderService>();
            services.AddScoped<IReportSerializer<ActivityReportModel>, ActivityReportSerializer>();
            services.AddScoped<IReportGenerator, ActivityReportGeneratorService>();

            // CurrentShopItemsReport Services
            services.AddScoped<IConfigurationLoader<CurrentShopItemsConfiguration>, CurrentShopItemsConfigurationLoaderService>();
            services.AddScoped<ICollectionReportSerializer<CurrentShopItemsModel>, CurrentShopItemsSerializer>();
            services.AddScoped<IReportGenerator, CurrentShopItemsReportGeneratorService>();
        }
    }
}
