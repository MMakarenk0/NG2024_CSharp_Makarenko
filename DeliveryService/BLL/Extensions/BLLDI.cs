using BLL.Mapping;
using BLL.Services.Classes;
using BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Extensions;

public static class BLLDI
{
    public static void AddBusinessLogicLayer(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutomapperBLLProfile));
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IManagerService, ManagerService>();
        services.AddScoped<IStorageService, StorageService>();
    }
}
