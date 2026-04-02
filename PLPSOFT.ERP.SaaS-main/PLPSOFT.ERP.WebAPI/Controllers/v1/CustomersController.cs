using MediatR;
using Microsoft.AspNetCore.Mvc;
using src.PLPSOFT.ERP.Application.Features.Customers.Commands.CreateCustomer;
using src.PLPSOFT.ERP.Application.Features.Customers.Commands.DeleteCustomer;
using src.PLPSOFT.ERP.Application.Features.Customers.Commands.UpdateCustomer;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using src.PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerById;
using src.PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerList;
using System;
using System.Threading.Tasks;

namespace src.PLPSOFT.ERP.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetCustomerListQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
        {
            var created = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand command)
        {
            if (id != command.Id) return BadRequest();
            var updated = await _mediator.Send(command);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _mediator.Send(new DeleteCustomerCommand { Id = id });
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
