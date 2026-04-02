using MediatR;
using PLPSOFT.ERP.Infrastructure.Extensions;
using PLPSOFT.ERP.Infrastructure.Persistence.Handlers.Customers;

var builder = WebApplication.CreateBuilder(args);

// Register persistence (AppDbContext)
builder.Services.AddPersistence(builder.Configuration);

// Register MediatR handlers (Infrastructure contains handlers)
builder.Services.AddMediatR(typeof(GetCustomerListQueryHandler).Assembly);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
