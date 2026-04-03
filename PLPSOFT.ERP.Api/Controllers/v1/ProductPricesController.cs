using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLPSOFT.ERP.Application.Features.Pricing.Commands;
using PLPSOFT.ERP.Application.Features.Pricing.Queries;

namespace PLPSOFT.ERP.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPricesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductPricesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetProductPriceListQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductPriceCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
