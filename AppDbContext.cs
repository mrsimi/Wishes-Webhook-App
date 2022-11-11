using Microsoft.EntityFrameworkCore;

namespace wishes_app
{
    public class AppDbContext : DbContext
    {
        public DbSet<WishItem> WishItems {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite(connectionString:"Data Source=Wishapp.db");
            optionsBuilder.UseInMemoryDatabase(databaseName: "WishesAppDB");
        }
    }

}