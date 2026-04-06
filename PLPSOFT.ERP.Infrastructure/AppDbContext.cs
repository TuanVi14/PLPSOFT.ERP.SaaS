using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Sales.SaaS.V2026.Models;
namespace PLPSOFT.ERP.Sales.SaaS.V2026.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SupplierGroup> SupplierGroups { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.SupplierGroup)
            .WithMany(g => g.Suppliers)
            .HasForeignKey(s => s.SupplierGroupID);

        modelBuilder.Entity<Company>()
    .HasKey(x => x.CompanyID);
    }
}