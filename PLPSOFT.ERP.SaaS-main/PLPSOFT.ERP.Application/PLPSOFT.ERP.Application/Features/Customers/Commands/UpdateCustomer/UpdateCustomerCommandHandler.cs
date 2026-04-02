using MediatR;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly AppDbContext _db;

        public UpdateCustomerCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null) return null;

            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.Email = request.Email;
            entity.Phone = request.Phone;
            entity.TaxCode = request.TaxCode;
            entity.CustomerGroupId = request.CustomerGroupId;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = System.DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

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
