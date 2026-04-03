using MediatR;
using PLPSOFT.ERP.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace src.PLPSOFT.ERP.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly AppDbContext _db;

        public DeleteCustomerCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null) return false;

            _db.Customers.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
