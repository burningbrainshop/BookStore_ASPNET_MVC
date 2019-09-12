using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BookStore.Models
{
    public class BookStoreContext : DbContext
    {
        public DbSet<CategorySetting> CategorySettings { get; set; }
        public DbSet<ProviderSetting> ProviderSettings { get; set; }
        public DbSet<OrderStatusSetting> OrderStatusSettings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseTitle> PurchaseTitles { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<StockTitle> StockTitles { get; set; }
        public DbSet<StockDetail> StockDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleInApplication> RoleInApplications { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<ManagerInRole> ManagerInRoles { get; set; }
        public DbSet<NextBuy> NextBuys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockTitle>().HasRequired(s => s.purchaseInfo).WithMany(s => s.stockInfo).WillCascadeOnDelete(false);
        }
    }
}