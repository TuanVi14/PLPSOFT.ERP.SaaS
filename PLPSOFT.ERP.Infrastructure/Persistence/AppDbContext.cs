using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System;
using System.Collections.Generic;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.Infrastructure.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<CustomerGroup> CustomerGroups { get; set; }

    public virtual DbSet<CustomerGroupProductPrice> CustomerGroupProductPrices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

    public virtual DbSet<ProductUnit> ProductUnits { get; set; }

    public virtual DbSet<ProductUnitMapping> ProductUnitMappings { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierGroup> SupplierGroups { get; set; }

    public virtual DbSet<SystemType> SystemTypes { get; set; }

    public virtual DbSet<SystemTypeValue> SystemTypeValues { get; set; }

    public virtual DbSet<TaxRate> TaxRates { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.BranchName).HasMaxLength(255);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasIndex(e => e.BaseCurrencyId, "IX_Companies_BaseCurrencyID");

            entity.HasIndex(e => e.CountryId, "IX_Companies_CountryID");

            entity.HasIndex(e => e.IsActive, "IX_Companies_IsActive");

            entity.HasIndex(e => e.ProvinceId, "IX_Companies_ProvinceID");

            entity.HasIndex(e => e.CompanyCode, "UQ_Companies_CompanyCode").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.AddressLine).HasMaxLength(255);
            entity.Property(e => e.BaseCurrencyId).HasColumnName("BaseCurrencyID");
            entity.Property(e => e.BusinessLicenseNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompanyCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName).HasMaxLength(255);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LegalName).HasMaxLength(255);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.ShortName).HasMaxLength(100);
            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasPrecision(0);
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.CustomerGroupId, "IX_Customers_CustomerGroupID");

            entity.HasIndex(e => new { e.CompanyId, e.CustomerCode }, "UX_Customers_Company_CustomerCode")
                .IsUnique()
                .HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerGroupId).HasColumnName("CustomerGroupID");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Facebook).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LoyaltyPoint).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Zalo).HasMaxLength(50);

            entity.HasOne(d => d.Company).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Companies");

            entity.HasOne(d => d.CustomerGroup).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerGroupId)
                .HasConstraintName("FK_Customers_CustomerGroups");

            entity.HasOne(d => d.CustomerType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_SystemTypeValues_CustomerType");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId);

            entity.HasIndex(e => e.CustomerId, "IX_CustomerAddresses_CustomerID");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.Latitude).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.ReceiverName).HasMaxLength(255);
            entity.Property(e => e.Ward).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerAddresses_Customers");
        });

        modelBuilder.Entity<CustomerGroup>(entity =>
        {
            entity.HasIndex(e => new { e.CompanyId, e.GroupCode }, "UX_CustomerGroups_Company_GroupCode").IsUnique();

            entity.Property(e => e.CustomerGroupId).HasColumnName("CustomerGroupID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GroupCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Company).WithMany(p => p.CustomerGroups)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerGroups_Companies");
        });

        modelBuilder.Entity<CustomerGroupProductPrice>(entity =>
        {
            entity.HasKey(e => e.GroupPriceId);

            entity.HasIndex(e => e.CustomerGroupId, "IX_CustomerGroupProductPrices_CustomerGroupID");

            entity.HasIndex(e => e.ProductId, "IX_CustomerGroupProductPrices_ProductID");

            entity.Property(e => e.GroupPriceId).HasColumnName("GroupPriceID");
            entity.Property(e => e.CustomerGroupId).HasColumnName("CustomerGroupID");
            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.CustomerGroup).WithMany(p => p.CustomerGroupProductPrices)
                .HasForeignKey(d => d.CustomerGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerGroupProductPrices_CustomerGroups");

            entity.HasOne(d => d.Product).WithMany(p => p.CustomerGroupProductPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerGroupProductPrices_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.BaseUnitId, "IX_Products_BaseUnitID");

            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryID");

            entity.HasIndex(e => new { e.CompanyId, e.ProductCode }, "UX_Products_Company_ProductCode")
                .IsUnique()
                .HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BaseUnitId).HasColumnName("BaseUnitID");
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CostPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DefaultTaxRateId).HasColumnName("DefaultTaxRateID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaxStock).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.MinStock).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.Origin).HasMaxLength(100);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SKU");
            entity.Property(e => e.StandardPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrackInventory).HasDefaultValue(true);
            entity.Property(e => e.Volume).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.Weight).HasColumnType("decimal(18, 3)");

            entity.HasOne(d => d.BaseUnit).WithMany(p => p.Products)
                .HasForeignKey(d => d.BaseUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_ProductUnits_BaseUnit");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_ProductCategories");

            entity.HasOne(d => d.Company).WithMany(p => p.Products)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Companies");

            entity.HasOne(d => d.DefaultTaxRate).WithMany(p => p.Products)
                .HasForeignKey(d => d.DefaultTaxRateId)
                .HasConstraintName("FK_Products_TaxRates_DefaultTaxRate");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_SystemTypeValues_ProductType");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.HasIndex(e => new { e.CompanyId, e.CategoryCode }, "UX_ProductCategories_Company_CategoryCode").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");

            entity.HasOne(d => d.Company).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductCategories_Companies");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_ProductCategories_Parent");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.PriceId);

            entity.HasIndex(e => e.BranchId, "IX_ProductPrices_BranchID");

            entity.HasIndex(e => e.CompanyId, "IX_ProductPrices_CompanyID");

            entity.HasIndex(e => e.ProductId, "IX_ProductPrices_ProductID");

            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Branch).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPrices_Branches");

            entity.HasOne(d => d.Company).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPrices_Companies");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPrices_Products");
        });

        modelBuilder.Entity<ProductUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId);

            entity.HasIndex(e => e.UnitCode, "UX_ProductUnits_UnitCode").IsUnique();

            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UnitCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UnitName).HasMaxLength(100);
            entity.Property(e => e.UnitTypeId).HasColumnName("UnitTypeID");

            entity.HasOne(d => d.UnitType).WithMany(p => p.ProductUnits)
                .HasForeignKey(d => d.UnitTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductUnits_SystemTypeValues_UnitType");
        });

        modelBuilder.Entity<ProductUnitMapping>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.UnitId });

            entity.HasIndex(e => e.UnitId, "IX_ProductUnitMappings_UnitID");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.ConversionRate).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductUnitMappings)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductUnitMappings_Products");

            entity.HasOne(d => d.Unit).WithMany(p => p.ProductUnitMappings)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductUnitMappings_ProductUnits");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(e => e.SupplierGroupId, "IX_Suppliers_SupplierGroupID");

            entity.HasIndex(e => new { e.CompanyId, e.SupplierCode }, "UX_Suppliers_Company_SupplierCode")
                .IsUnique()
                .HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierGroupId).HasColumnName("SupplierGroupID");
            entity.Property(e => e.SupplierName).HasMaxLength(255);
            entity.Property(e => e.SupplierTypeId).HasColumnName("SupplierTypeID");
            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_Companies");

            entity.HasOne(d => d.SupplierGroup).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.SupplierGroupId)
                .HasConstraintName("FK_Suppliers_SupplierGroups");

            entity.HasOne(d => d.SupplierType).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.SupplierTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Suppliers_SystemTypeValues_SupplierType");
        });

        modelBuilder.Entity<SupplierGroup>(entity =>
        {
            entity.HasIndex(e => new { e.CompanyId, e.GroupCode }, "UX_SupplierGroups_Company_GroupCode").IsUnique();

            entity.Property(e => e.SupplierGroupId).HasColumnName("SupplierGroupID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.GroupCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Company).WithMany(p => p.SupplierGroups)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierGroups_Companies");
        });

        modelBuilder.Entity<SystemType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.HasIndex(e => e.TypeCode, "UQ_SystemTypes_TypeCode").IsUnique();

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TypeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeName).HasMaxLength(150);
        });

        modelBuilder.Entity<SystemTypeValue>(entity =>
        {
            entity.HasKey(e => e.TypeValueId);

            entity.HasIndex(e => new { e.TypeId, e.ValueCode }, "UX_SystemTypeValues_Type_ValueCode").IsUnique();

            entity.Property(e => e.TypeValueId).HasColumnName("TypeValueID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.ValueCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ValueName).HasMaxLength(150);

            entity.HasOne(d => d.Type).WithMany(p => p.SystemTypeValues)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemTypeValues_SystemTypes");
        });

        modelBuilder.Entity<TaxRate>(entity =>
        {
            entity.HasIndex(e => new { e.CompanyId, e.TaxCode, e.EffectiveFrom }, "UX_TaxRates_Company_TaxCode_EffectiveFrom").IsUnique();

            entity.Property(e => e.TaxRateId).HasColumnName("TaxRateID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Rate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TaxCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TaxName).HasMaxLength(100);

            entity.HasOne(d => d.Company).WithMany(p => p.TaxRates)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaxRates_Companies");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
