using Microsoft.AspNetCore.Mvc;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductPricesController : Controller
    {
        private readonly AppDbContext _context;

        public ProductPricesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: List
        public async Task<IActionResult> Index()
        {
            var data = await _context.ProductPrices.ToListAsync();
            return View(data);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductPrice model)
        {
            if (ModelState.IsValid)
            {
                _context.ProductPrices.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(long id)
        {
            var data = await _context.ProductPrices.FindAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductPrice model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductPriceExists(model.PriceId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var entity = await _context.ProductPrices.FindAsync(id);
            if (entity != null)
            {
                _context.ProductPrices.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductPriceExists(long id)
        {
            return _context.ProductPrices.Any(e => e.PriceId == id);
        }
    }
}