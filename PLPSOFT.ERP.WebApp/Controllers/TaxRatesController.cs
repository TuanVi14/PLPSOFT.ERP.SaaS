using Microsoft.AspNetCore.Mvc;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class TaxRatesController : Controller
    {
        private readonly AppDbContext _context;

        public TaxRatesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TaxRates.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaxRate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.TaxRates.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var data = await _context.TaxRates.FindAsync(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaxRate model)
        {
            _context.TaxRates.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
