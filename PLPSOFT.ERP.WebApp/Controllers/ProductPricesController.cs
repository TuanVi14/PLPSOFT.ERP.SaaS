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

        public class IdRequest
        {
            public long Id { get; set; }
        }

        // ================= LIST =================
        public async Task<IActionResult> Index(string keyword)
        {
            var now = DateTime.Now;

            var query = _context.ProductPrices
                .Include(x => x.Product)
                .Include(x => x.Branch)
                .Include(x => x.Company)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                var keywordLower = keyword.ToLower();

                DateTime parsedDate;
                bool isDate = DateTime.TryParse(keyword, out parsedDate);

                query = query.Where(x =>
                    x.Price.ToString().Contains(keywordLower) ||

                    (x.Product != null && x.Product.ProductName.Contains(keyword)) ||
                    (x.Branch != null && x.Branch.BranchName.Contains(keyword)) ||
                    (x.Company != null && x.Company.CompanyName.Contains(keyword)) ||

                    (isDate && x.EffectiveFrom.Date == parsedDate.Date) ||
                    (isDate && x.EffectiveTo.HasValue && x.EffectiveTo.Value.Date == parsedDate.Date)
                );
            }

            // ✅ FIX: sort để deleted xuống dưới
            query = query
                .OrderBy(x => x.EffectiveTo.HasValue && x.EffectiveTo <= now)
                .ThenByDescending(x => x.EffectiveFrom);

            var data = await query.ToListAsync();
            return View(data);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductPrice model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(model);
            }

            _context.ProductPrices.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(long id)
        {
            var data = await _context.ProductPrices.FindAsync(id);

            if (data == null)
                return NotFound();

            LoadDropdowns();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductPrice model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(model);
            }

            var entity = await _context.ProductPrices.FindAsync(model.PriceId);

            if (entity == null)
                return NotFound();

            // ===== UPDATE BASIC FIELDS =====
            entity.ProductId = model.ProductId;
            entity.BranchId = model.BranchId;
            entity.CompanyId = model.CompanyId;
            entity.Price = model.Price;
            entity.EffectiveFrom = model.EffectiveFrom;

   
            var now = DateTime.Now;

     
            if (!(entity.EffectiveTo.HasValue && entity.EffectiveTo.Value <= now))
            {
                entity.EffectiveTo = model.EffectiveTo;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE (SOFT DELETE) =================
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] IdRequest request)
        {
            var entity = await _context.ProductPrices.FindAsync(request.Id);
            if (entity == null)
                return Json(new { success = false });

            entity.EffectiveTo = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ================= RESTORE =================
        [HttpPost]
        public async Task<IActionResult> Restore([FromBody] IdRequest request)
        {
            var entity = await _context.ProductPrices.FindAsync(request.Id);
            if (entity == null)
                return Json(new { success = false });

            entity.EffectiveTo = null; // restore

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ================= HELPER =================
        private void LoadDropdowns()
        {
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Branches = _context.Branches.ToList();
            ViewBag.Companies = _context.Companies.ToList();
        }
    }
}