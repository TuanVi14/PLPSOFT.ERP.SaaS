using MediatR;
using PLPSOFT.ERP.Application.Features.Customers.DTOs;
using System;

namespace PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
    }
}
