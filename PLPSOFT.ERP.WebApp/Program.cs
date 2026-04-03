using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PLPSOFT.ERP.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Lay chuoi ket noi tu file cau hinh
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration.GetConnectionString("PLPSOFTERPWebAppContext");

// Dang ky tat ca Context dung chung mot chuoi ket noi
builder.Services.AddDbContext<PLPSOFTERPWebAppContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<PLPSOFT.ERP.Infrastructure.Persistence.AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<PLPSOFT.ERP.Sales.SaaS.V2026.Data.AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();