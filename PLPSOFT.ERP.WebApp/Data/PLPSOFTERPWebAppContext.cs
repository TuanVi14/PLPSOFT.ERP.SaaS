using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.WebApp.Data
{
    public class PLPSOFTERPWebAppContext : DbContext
    {
        public PLPSOFTERPWebAppContext(DbContextOptions<PLPSOFTERPWebAppContext> options) : base(options) { }

        public DbSet<CustomerGroupProductPrice> CustomerGroupProductPrice { get; set; } = default!;
        public DbSet<ProductUnitMapping> ProductUnitMapping { get; set; } = default!;
        public DbSet<CustomerGroup> CustomerGroups { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<CustomerAddress> CustomerAddresses { get; set; } = default!;
        public DbSet<ProductUnit> Units { get; set; } = default!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix loi thieu precision cho kieu decimal
            modelBuilder.Entity<Customer>().Property(e => e.CreditLimit).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<CustomerGroupProductPrice>().Property(e => e.DiscountRate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<CustomerGroupProductPrice>().Property(e => e.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>().Property(e => e.StandardPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ProductUnitMapping>().Property(e => e.ConversionRate).HasColumnType("decimal(18,2)");
        }
    }

    public class AppDbContext : PLPSOFTERPWebAppContext
    {
        public AppDbContext(DbContextOptions<PLPSOFTERPWebAppContext> options) : base(options) { }
    }
}