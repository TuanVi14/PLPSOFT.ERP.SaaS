using MediatR;
using PLPSOFT.ERP.Application.Features.Customers.DTOs;
using System;

namespace PLPSOFT.ERP.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxCode { get; set; }
        public Guid? CustomerGroupId { get; set; }
    }
}
