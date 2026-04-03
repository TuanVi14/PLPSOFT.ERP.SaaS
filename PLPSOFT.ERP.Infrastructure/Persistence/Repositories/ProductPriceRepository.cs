using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Application.Features.Pricing.Repositories;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Repositories
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly AppDbContext _context;

        public ProductPriceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductPrice>> GetAllAsync()
        {
            return await _context.ProductPrices.ToListAsync();
        }
        public async Task AddAsync(ProductPrice entity)
        {
            await _context.ProductPrices.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
