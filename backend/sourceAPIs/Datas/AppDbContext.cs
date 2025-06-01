using Microsoft.EntityFrameworkCore;
using testVue.Models;

namespace testVue.Datas
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<FoodCategoryMdl> FoodCategorys { get; set; }
        public DbSet<FoodItemMdl> FoodItems { get; set; }
        public DbSet<OrderDetailMdl> OrderDetails { get; set; }
        public DbSet<OrderMdl> Orders { get; set; }
        public DbSet<ScheduleMdl> Schedules { get; set; }
        public DbSet<ShiftMdl> Shifts { get; set; } 
        public DbSet<TableMdl> Tables { get; set; }
        public DbSet<UserMdl> Users { get; set; }
    }
}
