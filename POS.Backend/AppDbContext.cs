using Microsoft.EntityFrameworkCore;
using POS.Backend.Models;

namespace POS.Backend
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        // same name as table
        public DbSet<ProductDataModel> Products { get; set; }
    }
}
