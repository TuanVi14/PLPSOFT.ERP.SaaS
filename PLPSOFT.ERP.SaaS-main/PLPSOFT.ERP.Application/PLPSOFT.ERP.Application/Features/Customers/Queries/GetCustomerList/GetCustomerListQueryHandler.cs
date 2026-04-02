using MediatR;
using Microsoft.EntityFrameworkCore;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using PLPSOFT.ERP.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, IEnumerable<CustomerDto>>
    {
        private readonly AppDbContext _db;

        public GetCustomerListQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CustomerDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var list = await _db.Customers
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(e => new CustomerDto
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    TaxCode = e.TaxCode,
                    CustomerGroupId = e.CustomerGroupId,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}
