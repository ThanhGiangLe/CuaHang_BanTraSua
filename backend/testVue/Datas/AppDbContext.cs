using Microsoft.EntityFrameworkCore;
using testVue.Models;
using testVue.Models.ChatBot;

namespace testVue.Datas
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserDTO> Users { get; set; } 
        public DbSet<FoodItemDTO> FoodItems { get; set; }
        public DbSet<FoodCategoryDTO> FoodCategories { get; set; }
        public DbSet<AdditionalFoodDTO> AdditionalFoods { get; set; }
        public DbSet<TableDTO> Tables { get; set; }
        public DbSet<OrderDTO> Orders { get; set; }
        public DbSet<OrderItemDTO> OrderItems { get; set; }
        public DbSet<MaterialDTO> Materials { get; set; }
        public DbSet<WarehouseHistoryDTO> WarehouseHistorys { get; set; }
        public DbSet<AImodel> AImodels { get; set; }
        public DbSet<CashRegisterDTO> CashRegisters { get; set; }
    }
}
