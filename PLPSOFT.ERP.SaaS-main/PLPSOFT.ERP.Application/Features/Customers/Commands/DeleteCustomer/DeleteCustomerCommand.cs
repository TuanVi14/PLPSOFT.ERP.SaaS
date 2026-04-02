using MediatR;
using System;

namespace PLPSOFT.ERP.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
