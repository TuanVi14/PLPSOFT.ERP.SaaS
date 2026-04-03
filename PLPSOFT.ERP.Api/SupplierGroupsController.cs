using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Sales.SaaS.V2026.Data;
using PLPSOFT.ERP.Sales.SaaS.V2026.Models;

namespace PLPSOFT.ERP.Sales.SaaS.V2026.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierGroupsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SupplierGroupsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.SupplierGroups.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierGroup model)
        {
            _context.SupplierGroups.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, SupplierGroup model)
        {
            var data = await _context.SupplierGroups.FindAsync(id);
            if (data == null) return NotFound();

            data.GroupName = model.GroupName;
            data.GroupCode = model.GroupCode;

            await _context.SaveChangesAsync();
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var data = await _context.SupplierGroups.FindAsync(id);
            if (data == null) return NotFound();

            _context.SupplierGroups.Remove(data);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
