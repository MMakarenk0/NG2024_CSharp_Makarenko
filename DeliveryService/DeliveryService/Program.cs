using DataLayer;
using DataLayer.Data.Infrastructure;
using BLL.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataAccessLayer(configuration);
builder.Services.AddBusinessLogicLayer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<DeliveryServiceDbContext>();
        DbInitializer.InitializeDatabase(context);

        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        var dataSeeder = new DataSeeder(unitOfWork);
        await dataSeeder.Seed();
    }
    catch (Exception)
    {
        throw;
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
