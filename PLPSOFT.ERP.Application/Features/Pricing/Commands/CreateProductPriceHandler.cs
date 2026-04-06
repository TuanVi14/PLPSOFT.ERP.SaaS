using MediatR;
using PLPSOFT.ERP.Application.Features.Pricing.Repositories;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.Application.Features.Pricing.Commands.CreateProductPrice
{
    public class CreateProductPriceHandler
        : IRequestHandler<CreateProductPriceCommand, long>
    {
        private readonly IProductPriceRepository _repo;

        public CreateProductPriceHandler(IProductPriceRepository repo)
        {
            _repo = repo;
        }

        public async Task<long> Handle(
            CreateProductPriceCommand request,
            CancellationToken cancellationToken)
        {
            var entity = new ProductPrice
            {
                ProductId = request.ProductId,
                BranchId = request.BranchId,
                CompanyId = request.CompanyId,
                Price = request.Price,
                EffectiveFrom = request.EffectiveFrom
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return entity.PriceId;
        }
    }
}