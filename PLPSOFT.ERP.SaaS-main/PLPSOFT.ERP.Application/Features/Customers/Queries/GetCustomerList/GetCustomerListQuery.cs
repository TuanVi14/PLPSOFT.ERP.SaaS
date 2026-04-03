using MediatR;
using PLPSOFT.ERP.Application.Features.Customers.DTOs;
using System.Collections.Generic;

namespace PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQuery : IRequest<IEnumerable<CustomerDto>>
    {
    }
}
