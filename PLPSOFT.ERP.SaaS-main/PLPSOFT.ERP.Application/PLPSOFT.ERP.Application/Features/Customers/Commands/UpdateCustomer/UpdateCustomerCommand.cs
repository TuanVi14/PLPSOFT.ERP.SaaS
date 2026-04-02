using MediatR;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using System;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxCode { get; set; }
        public Guid? CustomerGroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
