using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.Sales.SaaS.V2026.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SupplierGroup> SupplierGroups { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<CustomerGroupProductPrice> CustomerGroupProductPrices { get; set; }
    public DbSet<ProductUnitMapping> ProductUnitMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.SupplierGroup)
            .WithMany(g => g.Suppliers)
            .HasForeignKey(s => s.SupplierGroupID);
    }
}