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

        // ================= INDEX =================
        public async Task<IActionResult> Index(string keyword)
        {

            var query = _context.TaxRates
                .Include(x => x.Company) 
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(x =>
                    x.TaxCode.Contains(keyword) ||
                    x.TaxName.Contains(keyword) ||
                    x.Company.CompanyName.Contains(keyword)
                );

                // search số riêng
                if (decimal.TryParse(keyword, out var rate))
                {
                    query = query.Where(x => x.Rate == rate);
                }
            }

            var data = await query
                .OrderByDescending(x => x.IsActive)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(data);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            ViewBag.Companies = _context.Companies
                .Where(x => x.IsActive)
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaxRate model)
        {
            if (model.CompanyId <= 0)
            {
                ModelState.AddModelError("CompanyId", "Phải chọn công ty");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Companies = _context.Companies
                    .Where(x => x.IsActive)
                    .ToList();

                return View(model);
            }

            // luôn active khi tạo
            model.IsActive = true;
            model.CreatedAt = DateTime.Now;

            _context.TaxRates.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(long id)
        {
            var data = await _context.TaxRates.FindAsync(id);

            if (data == null)
                return NotFound();

            ViewBag.Companies = _context.Companies
                .Where(x => x.IsActive)
                .ToList();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaxRate model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Companies = _context.Companies
                    .Where(x => x.IsActive)
                    .ToList();

                return View(model);
            }

            var entity = await _context.TaxRates.FindAsync(model.TaxRateId);
            if (entity == null)
                return NotFound();

            // map dữ liệu (KHÔNG chỉnh IsActive)
            entity.CompanyId = model.CompanyId;
            entity.TaxCode = model.TaxCode;
            entity.TaxName = model.TaxName;
            entity.Rate = model.Rate;
            entity.EffectiveFrom = model.EffectiveFrom;
            entity.EffectiveTo = model.EffectiveTo;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE (SOFT DELETE) =================
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            var entity = await _context.TaxRates.FindAsync(request.Id);

            if (entity == null)
            {
                return Json(new { success = false });
            }

            entity.IsActive = false;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ================= RESTORE =================
        [HttpPost]
        public async Task<IActionResult> Restore([FromBody] DeleteRequest request)
        {
            var entity = await _context.TaxRates.FindAsync(request.Id);

            if (entity == null)
            {
                return Json(new { success = false });
            }

            entity.IsActive = true;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public class DeleteRequest
        {
            public long Id { get; set; } 
        }
    }
}