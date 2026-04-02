using MediatR;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using System.Collections.Generic;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQuery : IRequest<IEnumerable<CustomerDto>>
    {
    }
}
