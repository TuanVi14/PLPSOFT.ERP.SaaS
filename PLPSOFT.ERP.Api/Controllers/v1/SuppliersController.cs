using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;


namespace PLPSOFT.ERP.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Suppliers
                .Include(s => s.SupplierGroup)
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier model)
        {
            model.CreatedAt = DateTime.Now;

            _context.Suppliers.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, Supplier model)
        {
            var data = await _context.Suppliers.FindAsync(id);
            if (data == null) return NotFound();

            data.SupplierName = model.SupplierName;
            data.Phone = model.Phone;
            data.Email = model.Email;

            await _context.SaveChangesAsync();
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var data = await _context.Suppliers.FindAsync(id);
            if (data == null) return NotFound();

            _context.Suppliers.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
