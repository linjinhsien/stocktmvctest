using Microsoft.EntityFrameworkCore;

namespace WebApplication2.partials
{
    public partial class StockContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration Config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsetting.json").Build();
                optionsBuilder.UseSqlServer(Config.GetConnectionString("STOCK"));
            }
        }

    }
}
