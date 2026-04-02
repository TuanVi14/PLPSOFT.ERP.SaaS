using MediatR;
using Microsoft.EntityFrameworkCore;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using PLPSOFT.ERP.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly AppDbContext _db;

        public GetCustomerByIdQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var e = await _db.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (e == null) return null;

            return new CustomerDto
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
            };
        }
    }
}
