using MediatR;
using PLPSOFT.ERP.Application.Features.Customers.Commands.DeleteCustomer;
using PLPSOFT.ERP.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Handlers.Customers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly AppDbContext _db;

        public DeleteCustomerCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity != null)
            {
                _db.Customers.Remove(entity);
                await _db.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
