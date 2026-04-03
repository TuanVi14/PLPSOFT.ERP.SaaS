using MediatR;
using PLPSOFT.ERP.Application.Features.Customers.DTOs;
using PLPSOFT.ERP.Application.Features.Customers.Queries.GetCustomerById;
using PLPSOFT.ERP.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Handlers.Customers
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
            var entity = await _db.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null) return null;

            return new CustomerDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                TaxCode = entity.TaxCode,
                CustomerGroupId = entity.CustomerGroupId,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
