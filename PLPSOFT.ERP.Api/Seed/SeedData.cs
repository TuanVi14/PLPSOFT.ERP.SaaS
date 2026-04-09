using System;
using System.Linq;
using System.Threading.Tasks;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.Api.Seed
{
    public static class SeedData
    {
        public static async Task EnsureSeedAsync(AppDbContext db)
        {
            // create database if not exists
            db.Database.EnsureCreated();

            if (!db.Companies.Any())
            {
                var company = new Company { CompanyName = "Demo Company", CompanyCode = "DEMO" };
                db.Companies.Add(company);
                await db.SaveChangesAsync();
            }

            if (!db.ProductUnits.Any())
            {
                // Ensure SystemType and SystemTypeValues for UnitType exist
                var unitType = db.SystemTypes.FirstOrDefault(st => st.TypeCode == "UNIT_TYPE");
                if (unitType == null)
                {
                    unitType = new SystemType { TypeCode = "UNIT_TYPE", TypeName = "Unit Type", IsActive = true };
                    db.SystemTypes.Add(unitType);
                    await db.SaveChangesAsync();

                    db.SystemTypeValues.Add(new SystemTypeValue { TypeId = unitType.TypeId, ValueCode = "BASE", ValueName = "Base Unit", SortOrder = 1, IsActive = true });
                    db.SystemTypeValues.Add(new SystemTypeValue { TypeId = unitType.TypeId, ValueCode = "OTHER", ValueName = "Other", SortOrder = 2, IsActive = true });
                    await db.SaveChangesAsync();
                }

                var baseUnitTypeId = db.SystemTypeValues.First(v => v.ValueCode == "BASE").TypeValueId;

                db.ProductUnits.Add(new ProductUnit { UnitCode = "EA", UnitName = "Each", UnitTypeId = baseUnitTypeId });
                db.ProductUnits.Add(new ProductUnit { UnitCode = "BOX", UnitName = "Box", UnitTypeId = baseUnitTypeId });
                await db.SaveChangesAsync();
            }

            if (!db.Products.Any())
            {
                // Ensure PRODUCT_TYPE exists
                var productType = db.SystemTypes.FirstOrDefault(st => st.TypeCode == "PRODUCT_TYPE");
                if (productType == null)
                {
                    productType = new SystemType { TypeCode = "PRODUCT_TYPE", TypeName = "Product Type", IsActive = true };
                    db.SystemTypes.Add(productType);
                    await db.SaveChangesAsync();

                    db.SystemTypeValues.Add(new SystemTypeValue { TypeId = productType.TypeId, ValueCode = "DEFAULT", ValueName = "Default", SortOrder = 1, IsActive = true });
                    await db.SaveChangesAsync();
                }

                var baseUnit = db.ProductUnits.First();
                var defaultProductTypeId = db.SystemTypeValues.First(v => v.TypeId == productType.TypeId && v.ValueCode == "DEFAULT").TypeValueId;

                db.Products.Add(new Product
                {
                    ProductCode = "P-001",
                    ProductName = "Demo Product",
                    CompanyId = db.Companies.First().CompanyId,
                    BaseUnitId = baseUnit.UnitId,
                    ProductTypeId = defaultProductTypeId,
                    CostPrice = 10,
                    StandardPrice = 15,
                    IsActive = true
                });
                await db.SaveChangesAsync();
            }

            if (!db.CustomerGroups.Any())
            {
                db.CustomerGroups.Add(new CustomerGroup { CompanyId = db.Companies.First().CompanyId, GroupCode = "RETAIL", GroupName = "Retail" });
                await db.SaveChangesAsync();
            }

            if (!db.Customers.Any())
            {
                // Ensure CUSTOMER_TYPE exists
                var customerType = db.SystemTypes.FirstOrDefault(st => st.TypeCode == "CUSTOMER_TYPE");
                if (customerType == null)
                {
                    customerType = new SystemType { TypeCode = "CUSTOMER_TYPE", TypeName = "Customer Type", IsActive = true };
                    db.SystemTypes.Add(customerType);
                    await db.SaveChangesAsync();

                    db.SystemTypeValues.Add(new SystemTypeValue { TypeId = customerType.TypeId, ValueCode = "DEFAULT", ValueName = "Default", SortOrder = 1, IsActive = true });
                    await db.SaveChangesAsync();
                }

                var defaultCustomerTypeId = db.SystemTypeValues.First(v => v.TypeId == customerType.TypeId && v.ValueCode == "DEFAULT").TypeValueId;

                var c = new Customer
                {
                    CompanyId = db.Companies.First().CompanyId,
                    CustomerCode = "CUST001",
                    CustomerName = "Khách hàng demo",
                    CustomerGroupId = db.CustomerGroups.First().CustomerGroupId,
                    CustomerTypeId = defaultCustomerTypeId,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                db.Customers.Add(c);
                await db.SaveChangesAsync();

                db.CustomerAddresses.Add(new CustomerAddress
                {
                    CustomerId = c.CustomerId,
                    ReceiverName = "Người nhận",
                    Phone = "0123456789",
                    Address = "123 Demo Street",
                    Province = "Hanoi",
                    District = "Ba Dinh",
                    Ward = "Phuc Xa",
                    IsDefault = true,
                    CreatedAt = DateTime.Now
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
