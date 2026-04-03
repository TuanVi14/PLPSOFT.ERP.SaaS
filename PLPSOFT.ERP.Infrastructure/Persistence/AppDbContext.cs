using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 🔥 DbSet
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<CustomerGroupProductPrice> CustomerGroupProductPrices { get; set; }
        public DbSet<ProductUnitMapping> ProductUnitMappings { get; set; }



        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }
        public IEnumerable ProductTypes { get; set; }

        // 🔥 Apply Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<Product>().Property(p => p.CompanyID).HasColumnName("CompanyID");
        }
    }
}
