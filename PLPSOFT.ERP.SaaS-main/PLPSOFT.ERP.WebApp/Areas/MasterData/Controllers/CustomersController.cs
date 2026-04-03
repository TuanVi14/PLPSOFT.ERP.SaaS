using MediatR;
using Microsoft.AspNetCore.Mvc;
using PLPSOFT.ERP.Application.Features.Customers.Commands.CreateCustomer;
using PLPSOFT.ERP.Application.Features.Customers.Commands.DeleteCustomer;
using PLPSOFT.ERP.Application.Features.Customers.Commands.UpdateCustomer;
using PLPSOFT.ERP.Application.Features.Customers.DTOs;
using PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerById;
using PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerList;
using System;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.WebApp.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _mediator.Send(new GetCustomerListQuery());
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new CustomerDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var cmd = new CreateCustomerCommand
            {
                Code = model.Code,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                TaxCode = model.TaxCode,
                CustomerGroupId = model.CustomerGroupId
            };

            var created = await _mediator.Send(cmd);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var dto = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, CustomerDto model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var cmd = new UpdateCustomerCommand
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                TaxCode = model.TaxCode,
                CustomerGroupId = model.CustomerGroupId,
                IsActive = model.IsActive
            };

            var updated = await _mediator.Send(cmd);
            if (updated == null) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var dto = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _mediator.Send(new DeleteCustomerCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
