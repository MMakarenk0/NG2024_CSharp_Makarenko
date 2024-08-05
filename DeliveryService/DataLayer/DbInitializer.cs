using DataLayer.Data.Infrastructure;

namespace DataLayer
{
    public class DbInitializer
    {
        public static void InitializeDatabase(DeliveryServiceDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
        }
    }
}
