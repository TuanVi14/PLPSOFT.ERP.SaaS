using PLPSOFT.ERP.Application.Features.Pricing.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PLPSOFT.ERP.Application.Features.Pricing.Queries
{
    public record GetProductPriceListQuery : IRequest<List<ProductPriceDto>>;
}
