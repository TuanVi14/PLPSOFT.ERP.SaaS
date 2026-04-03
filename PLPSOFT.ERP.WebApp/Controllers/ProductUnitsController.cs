using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.WebApp.Models;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductUnitsController : Controller
    {
        private readonly AppDbContext _context;
        public ProductUnitsController(AppDbContext context) => _context = context;

        // Hiển thị danh sách
        public async Task<IActionResult> Index()
        {
            var units = await _context.ProductUnits
                .Select(u => new ProductUnitViewModel
                {
                    ProductUnitID = u.ProductUnitID,
                    UnitCode = u.UnitCode,
                    UnitName = u.UnitName
                }).ToListAsync();
            return View(units);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductUnitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var unit = new ProductUnit
                {
                    
                    UnitCode = model.UnitCode,
                    UnitName = model.UnitName,
                    IsActive = true
                };
                _context.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Tương tự cho Edit và Delete...
    }
}