using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.WebApp.Data;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductUnitMappingController : Controller
    {
        private readonly PLPSOFTERPWebAppContext _context;

        public ProductUnitMappingController(PLPSOFTERPWebAppContext context)
        {
            _context = context;
        }

        // Load dropdown
        private void LoadDropdowns(long? productId = null, long? unitId = null)
        {
            ViewBag.ProductId = new SelectList(_context.Products, "ProductID", "ProductName", productId);
            ViewBag.UnitId = new SelectList(_context.Units, "ProductUnitID", "UnitName", unitId);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductUnitMapping.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            var productUnitMapping = await _context.ProductUnitMapping.FirstOrDefaultAsync(m => m.UnitMappingId == id);
            if (productUnitMapping == null) return NotFound();
            return View(productUnitMapping);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitMappingId,ProductId,UnitId,ConversionRate,IsDefault")] ProductUnitMapping productUnitMapping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productUnitMapping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns(productUnitMapping.ProductId, productUnitMapping.UnitId);
            return View(productUnitMapping);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();
            var productUnitMapping = await _context.ProductUnitMapping.FindAsync(id);
            if (productUnitMapping == null) return NotFound();

            LoadDropdowns(productUnitMapping.ProductId, productUnitMapping.UnitId);
            return View(productUnitMapping);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UnitMappingId,ProductId,UnitId,ConversionRate,IsDefault")] ProductUnitMapping productUnitMapping)
        {
            if (id != productUnitMapping.UnitMappingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productUnitMapping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductUnitMappingExists(productUnitMapping.UnitMappingId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns(productUnitMapping.ProductId, productUnitMapping.UnitId);
            return View(productUnitMapping);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();
            var productUnitMapping = await _context.ProductUnitMapping.FirstOrDefaultAsync(m => m.UnitMappingId == id);
            if (productUnitMapping == null) return NotFound();
            return View(productUnitMapping);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var productUnitMapping = await _context.ProductUnitMapping.FindAsync(id);
            if (productUnitMapping != null)
            {
                _context.ProductUnitMapping.Remove(productUnitMapping);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductUnitMappingExists(long id)
        {
            return _context.ProductUnitMapping.Any(e => e.UnitMappingId == id);
        }
    }
}