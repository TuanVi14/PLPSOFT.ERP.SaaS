using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Infrastructure.Persistence;

namespace PLPSOFT.ERP.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxRatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaxRatesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.TaxRates.ToListAsync();
            return Ok(data);
        }
    }
}
