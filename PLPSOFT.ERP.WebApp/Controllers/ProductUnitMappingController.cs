using System;
using System.Collections.Generic;
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

        // GET: ProductUnitMapping
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductUnitMapping.ToListAsync());
        }

        // GET: ProductUnitMapping/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productUnitMapping = await _context.ProductUnitMapping
                .FirstOrDefaultAsync(m => m.UnitMappingId == id);
            if (productUnitMapping == null)
            {
                return NotFound();
            }

            return View(productUnitMapping);
        }

        // GET: ProductUnitMapping/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductUnitMapping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            return View(productUnitMapping);
        }

        // GET: ProductUnitMapping/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productUnitMapping = await _context.ProductUnitMapping.FindAsync(id);
            if (productUnitMapping == null)
            {
                return NotFound();
            }
            return View(productUnitMapping);
        }

        // POST: ProductUnitMapping/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UnitMappingId,ProductId,UnitId,ConversionRate,IsDefault")] ProductUnitMapping productUnitMapping)
        {
            if (id != productUnitMapping.UnitMappingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productUnitMapping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductUnitMappingExists(productUnitMapping.UnitMappingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productUnitMapping);
        }

        // GET: ProductUnitMapping/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productUnitMapping = await _context.ProductUnitMapping
                .FirstOrDefaultAsync(m => m.UnitMappingId == id);
            if (productUnitMapping == null)
            {
                return NotFound();
            }

            return View(productUnitMapping);
        }

        // POST: ProductUnitMapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var productUnitMapping = await _context.ProductUnitMapping.FindAsync(id);
            if (productUnitMapping != null)
            {
                _context.ProductUnitMapping.Remove(productUnitMapping);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductUnitMappingExists(long id)
        {
            return _context.ProductUnitMapping.Any(e => e.UnitMappingId == id);
        }
    }
}
