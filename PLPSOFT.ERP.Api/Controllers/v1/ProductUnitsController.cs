using Microsoft.AspNetCore.Mvc;
using PLPSOFT.ERP.Infrastructure.Persistence.Repositories;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductUnitsController : ControllerBase
    {
        private readonly ProductUnitRepository _repo;
        public ProductUnitsController(ProductUnitRepository repo) => _repo = repo;

        [HttpGet("{companyId}")]
        public async Task<IActionResult> Get(long companyId)
        {
            var data = await _repo.GetAllAsync(companyId);
            return Ok(data);
        }
    }
}