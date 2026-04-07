using Microsoft.AspNetCore.Mvc;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    x.Price.ToString().Contains(keyword) ||

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
        public IActionResult Create(long? companyId)
        {
            var model = new ProductPrice
            {
                CompanyId = companyId ?? 0,
                EffectiveFrom = DateTime.Now
            };


            LoadDropdowns(model.CompanyId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductPrice model)
        {
            if (model.CompanyId == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn công ty");
            }

            if (model.EffectiveTo.HasValue && model.EffectiveTo < model.EffectiveFrom)
            {
                ModelState.AddModelError("", "EffectiveTo phải lớn hơn EffectiveFrom");
            }

            var branch = await _context.Branches
                .FirstOrDefaultAsync(x => x.BranchId == model.BranchId);

            if (branch == null || branch.CompanyId != model.CompanyId)
            {
                ModelState.AddModelError("", "Chi nhánh không thuộc công ty!");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == model.ProductId);

            if (product == null || product.CompanyId != model.CompanyId)
            {
                ModelState.AddModelError("", "Sản phẩm không thuộc công ty!");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.CompanyId);
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (IsOverlapping(model))
                {
                    ModelState.AddModelError("", "Khoảng thời gian bị trùng!");
                    LoadDropdowns(model.CompanyId);
                    return View(model);
                }

                var oldPrices = await _context.ProductPrices
                    .Where(x =>
                        x.ProductId == model.ProductId &&
                        x.BranchId == model.BranchId &&
                        x.CompanyId == model.CompanyId &&
                        x.EffectiveTo == null)
                    .ToListAsync();

                foreach (var old in oldPrices)
                {
                    var newTo = model.EffectiveFrom.AddSeconds(-1);

                    if (newTo < old.EffectiveFrom)
                        newTo = old.EffectiveFrom;

                    old.EffectiveTo = newTo;
                }

                _context.ProductPrices.Add(model);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(long id)
        {
            var data = await _context.ProductPrices.FindAsync(id);

            if (data == null)
                return NotFound();
            var vm = new ProductPrice
            {
                PriceId = data.PriceId,
                ProductId = data.ProductId,
                BranchId = data.BranchId,
                CompanyId = data.CompanyId,
                EffectiveTo = data.EffectiveTo,
                EffectiveFrom = data.EffectiveFrom,
                Price = data.Price,

            };
            LoadDropdowns(data.CompanyId);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductPrice model)
        {
            if (model.EffectiveTo.HasValue && model.EffectiveTo < model.EffectiveFrom)
            {
                ModelState.AddModelError("", "EffectiveTo phải lớn hơn EffectiveFrom");
            }

            var branch = await _context.Branches
                .FirstOrDefaultAsync(x => x.BranchId == model.BranchId);

            if (branch == null || branch.CompanyId != model.CompanyId)
            {
                ModelState.AddModelError("", "Chi nhánh không thuộc công ty!");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == model.ProductId);

            if (product == null || product.CompanyId != model.CompanyId)
            {
                ModelState.AddModelError("", "Sản phẩm không thuộc công ty!");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.CompanyId);
                return View(model);
            }

            var entity = await _context.ProductPrices.FindAsync(model.PriceId);

            if (entity == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                entity.ProductId = model.ProductId;
                entity.BranchId = model.BranchId;
                entity.CompanyId = model.CompanyId;
                entity.Price = model.Price;
                entity.EffectiveFrom = model.EffectiveFrom;
                entity.EffectiveTo = model.EffectiveTo;

                if (IsOverlapping(entity))
                {
                    ModelState.AddModelError("", "Khoảng thời gian bị trùng!");
                    LoadDropdowns(model.CompanyId);
                    return View(model);
                }

                var now = DateTime.Now;

                if (entity.EffectiveTo.HasValue && entity.EffectiveTo <= now)
                {
                    ModelState.AddModelError("", "Không thể sửa bản ghi đã hết hiệu lực!");
                    LoadDropdowns(model.CompanyId);
                    return View(model);
                }

                var otherPrices = await _context.ProductPrices
                    .Where(x =>
                        x.PriceId != entity.PriceId &&
                        x.ProductId == entity.ProductId &&
                        x.BranchId == entity.BranchId &&
                        x.CompanyId == entity.CompanyId)
                    .ToListAsync();

                foreach (var item in otherPrices)
                {
                    if (item.EffectiveTo == null)
                    {
                        var newTo = entity.EffectiveFrom.AddSeconds(-1);

                        if (newTo < item.EffectiveFrom)
                            newTo = item.EffectiveFrom;

                        item.EffectiveTo = newTo;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

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
        private void LoadDropdowns(long? companyId = null)
        {
            ViewBag.Companies = _context.Companies.ToList();

            if (companyId.HasValue && companyId != 0)
            {
                ViewBag.Branches = _context.Branches
                    .Where(x => x.CompanyId == companyId).ToList();

                ViewBag.Products = _context.Products
                    .Where(x => x.CompanyId == companyId).ToList();
            }
            else
            {
                ViewBag.Branches = new List<Branch>();
                ViewBag.Products = new List<Product>();
            }
        }

        private bool IsOverlapping(ProductPrice model)
        {
            return _context.ProductPrices.Any(x =>
                x.PriceId != model.PriceId &&
                x.ProductId == model.ProductId &&
                x.BranchId == model.BranchId &&
                x.CompanyId == model.CompanyId &&

                // overlap logic
                (model.EffectiveTo == null || x.EffectiveFrom <= model.EffectiveTo) &&
                (x.EffectiveTo == null || model.EffectiveFrom <= x.EffectiveTo)
            );
        }
    }
}