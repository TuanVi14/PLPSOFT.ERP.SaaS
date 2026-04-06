using Microsoft.AspNetCore.Mvc;

using PLPSOFT.ERP.Infrastructure.Persistence.Repositories;
namespace PLPSOFT.ERP.Api.Controllers.v1;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoriesController : ControllerBase
{
    private readonly ProductCategoryRepository _repo;
    public ProductCategoriesController(ProductCategoryRepository repo) => _repo = repo;

    [HttpGet("{companyId}")]
    public async Task<IActionResult> Get(long companyId)
    {
        var data = await _repo.GetAllByCompanyAsync(companyId);
        return Ok(data);
    }
}