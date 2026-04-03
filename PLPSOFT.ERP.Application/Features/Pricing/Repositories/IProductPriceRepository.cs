using PLPSOFT.ERP.Domain.Entities.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Application.Features.Pricing.Repositories
{
    public interface IProductPriceRepository
    {
        Task<List<ProductPrice>> GetAllAsync();
        Task AddAsync(ProductPrice entity);
        Task SaveChangesAsync();
    }
}
