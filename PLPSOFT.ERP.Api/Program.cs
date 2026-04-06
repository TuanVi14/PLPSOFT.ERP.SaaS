using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Application.Features.Pricing.Commands.CreateProductPrice;
using PLPSOFT.ERP.Application.Features.Pricing.Repositories;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// ?? Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductPriceHandler).Assembly));

// Repository
builder.Services.AddScoped<IProductPriceRepository, ProductPriceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();