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
        public async Task<IActionResult> Create(ProductPrice model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.ProductPrices.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public async Task<IActionResult> Edit(long id)
        {
            var data = await _context.ProductPrices.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(ProductPrice model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = await _context.ProductPrices.FindAsync(model.PriceId);

            if (entity == null)
            {
                return NotFound();
            }

            entity.ProductId = model.ProductId;
            entity.BranchId = model.BranchId;
            entity.CompanyId = model.CompanyId;
            entity.Price = model.Price;
            entity.EffectiveFrom = model.EffectiveFrom;
            entity.EffectiveTo = model.EffectiveTo;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE (optional)
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.ProductPrices.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            _context.ProductPrices.Remove(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}