using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PLPSOFT.ERP.Sales.SaaS.V2026.Data;

namespace PLPSOFT.ERP.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // Thay chuỗi kết nối bên dưới bằng chuỗi kết nối thực tế trong appsettings.json của bạn
        optionsBuilder.UseSqlServer("Server=.;Database=PLPSOFT_ERP;Trusted_Connection=True;TrustServerCertificate=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}