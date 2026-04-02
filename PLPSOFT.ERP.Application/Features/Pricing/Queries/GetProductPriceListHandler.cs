using MediatR;
using PLPSOFT.ERP.Application.Features.Pricing.DTOs;
using PLPSOFT.ERP.Application.Features.Pricing.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Application.Features.Pricing.Queries
{
    public class GetProductPriceListHandler
        : IRequestHandler<GetProductPriceListQuery, List<ProductPriceDto>>
    {
        private readonly IProductPriceRepository _repo;

        public GetProductPriceListHandler(IProductPriceRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProductPriceDto>> Handle(
            GetProductPriceListQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _repo.GetAllAsync();

            return data.Select(x => new ProductPriceDto
            {
                PriceId = x.PriceId,
                ProductId = x.ProductId,
                BranchId = x.BranchId,
                CompanyId = x.CompanyId,
                Price = x.Price,
                EffectiveFrom = x.EffectiveFrom
            }).ToList();
        }
    }
}
