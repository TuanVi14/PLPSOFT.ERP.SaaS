using MediatR;
using src.PLPSOFT.ERP.Application.Features.Customers.Commands.CreateCustomer;
using src.PLPSOFT.ERP.Application.Features.Customers.DTOs;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly AppDbContext _db;

        public CreateCustomerCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new Customer
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                TaxCode = request.TaxCode,
                CustomerGroupId = request.CustomerGroupId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Customers.Add(entity);
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
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
