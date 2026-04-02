using MediatR;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using System;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
    }
}
