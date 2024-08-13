using Microsoft.Extensions.DependencyInjection;
using ReportApp.Services;
using ReportApp.Services.ActivityReport;
using ReportApp.Services.CurrentItemsShop;

namespace ReportApp.Configuration
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<TemplateLoader>();
            services.AddScoped<ConfigurationLoader>();
            services.AddScoped<ReportSerializer>();
            services.AddScoped<ActivityReportGeneratorService>();
            services.AddScoped<CurrentShopItemsReportGeneratorService>();
        }
    }
}
