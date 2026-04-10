using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.WebApp.Models;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class CustomerGroupProductPricesController : Controller
    {
        private readonly AppDbContext _db;
        private const int DefaultPageSize = 20;

        public CustomerGroupProductPricesController(AppDbContext db) => _db = db;

        // ══════════════════════════════════════════════════════════════════════
        // GET: /CustomerGroupProductPrices
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Index(
            CustomerGroupProductPriceFilterViewModel filter,
            int page = 1)
        {
            var query = _db.CustomerGroupProductPrices
                .AsNoTracking()
                .Include(x => x.CustomerGroup)
                .Include(x => x.Product)
                .AsQueryable();

            // ── bộ lọc ──────────────────────────────────────────────────────
            if (filter.CustomerGroupID.HasValue)
                query = query.Where(x => x.CustomerGroupId == filter.CustomerGroupID);

            if (filter.ProductID.HasValue)
                query = query.Where(x => x.ProductId == filter.ProductID);

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var kw = filter.Keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.CustomerGroup.GroupCode.ToLower().Contains(kw) ||
                    x.CustomerGroup.GroupName.ToLower().Contains(kw) ||
                    x.Product.ProductCode.ToLower().Contains(kw) ||
                    x.Product.ProductName.ToLower().Contains(kw));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.IsActive)
                .ThenByDescending(x => x.EffectiveFrom)
                .Skip((page - 1) * DefaultPageSize)
                .Take(DefaultPageSize)
                .Select(x => new CustomerGroupProductPriceRowViewModel
                {
                    GroupPriceID = x.GroupPriceId,
                    CustomerGroupID = x.CustomerGroupId,
                    GroupCode = x.CustomerGroup.GroupCode,
                    GroupName = x.CustomerGroup.GroupName,
                    ProductID = x.ProductId,
                    ProductCode = x.Product.ProductCode,
                    ProductName = x.Product.ProductName,
                    Price = x.Price,
                    DiscountRate = x.DiscountRate,
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            var vm = new CustomerGroupProductPriceListViewModel
            {
                Items = items,
                Filter = filter,
                TotalCount = totalCount,
                PageIndex = page,
                PageSize = DefaultPageSize
            };

            await PopulateFilterDropdowns(filter);
            return View(vm);
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: /CustomerGroupProductPrices/Details/5
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Details(long id)
        {
            var entity = await _db.CustomerGroupProductPrices
                .AsNoTracking()
                .Include(x => x.CustomerGroup)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.GroupPriceId == id);

            if (entity is null) return NotFound();

            var vm = MapToRow(entity);
            return View(vm);
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: /CustomerGroupProductPrices/Create
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Create()
        {
            var vm = new CustomerGroupProductPriceFormViewModel();
            await PopulateFormDropdowns(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────────────────────────────
        // POST: /CustomerGroupProductPrices/Create
        // ──────────────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerGroupProductPriceFormViewModel vm)
        {
            // validate tùy biến (giá hoặc chiết khấu phải có ít nhất 1)
            CustomValidate(vm);

            if (!ModelState.IsValid)
            {
                await PopulateFormDropdowns(vm);
                return View(vm);
            }

            // kiểm tra trùng (cùng nhóm + sản phẩm + khoảng thời gian chồng chéo)
            if (await HasOverlapAsync(vm.CustomerGroupID, vm.ProductID, vm.EffectiveFrom, vm.EffectiveTo, null))
            {
                ModelState.AddModelError(string.Empty,
                    "Đã tồn tại bản ghi giá cho nhóm & sản phẩm này trong khoảng thời gian trùng nhau.");
                await PopulateFormDropdowns(vm);
                return View(vm);
            }

            var entity = new CustomerGroupProductPrice
            {
                CustomerGroupId = vm.CustomerGroupID,
                ProductId = vm.ProductID,
                Price = vm.Price,
                DiscountRate = vm.DiscountRate,
                EffectiveFrom = vm.EffectiveFrom,
                EffectiveTo = vm.EffectiveTo,
                IsActive = vm.IsActive
            };

            _db.CustomerGroupProductPrices.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Tạo giá nhóm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: /CustomerGroupProductPrices/Edit/5
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _db.CustomerGroupProductPrices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GroupPriceId == id);

            if (entity is null) return NotFound();

            var vm = new CustomerGroupProductPriceFormViewModel
            {
                GroupPriceID = entity.GroupPriceId,
                CustomerGroupID = entity.CustomerGroupId,
                ProductID = entity.ProductId,
                Price = entity.Price,
                DiscountRate = entity.DiscountRate,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                IsActive = entity.IsActive
            };

            await PopulateFormDropdowns(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────────────────────────────
        // POST: /CustomerGroupProductPrices/Edit/5
        // ──────────────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, CustomerGroupProductPriceFormViewModel vm)
        {
            if (id != vm.GroupPriceID) return BadRequest();

            CustomValidate(vm);

            if (!ModelState.IsValid)
            {
                await PopulateFormDropdowns(vm);
                return View(vm);
            }

            var entity = await _db.CustomerGroupProductPrices
                .FirstOrDefaultAsync(x => x.GroupPriceId == id);

            if (entity is null) return NotFound();

            if (await HasOverlapAsync(vm.CustomerGroupID, vm.ProductID, vm.EffectiveFrom, vm.EffectiveTo, id))
            {
                ModelState.AddModelError(string.Empty,
                    "Khoảng thời gian bị trùng với bản ghi khác trong cùng nhóm & sản phẩm.");
                await PopulateFormDropdowns(vm);
                return View(vm);
            }

            entity.CustomerGroupId = vm.CustomerGroupID;
            entity.ProductId = vm.ProductID;
            entity.Price = vm.Price;
            entity.DiscountRate = vm.DiscountRate;
            entity.EffectiveFrom = vm.EffectiveFrom;
            entity.EffectiveTo = vm.EffectiveTo;
            entity.IsActive = vm.IsActive;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: /CustomerGroupProductPrices/Delete/5
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _db.CustomerGroupProductPrices
                .AsNoTracking()
                .Include(x => x.CustomerGroup)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.GroupPriceId == id);

            if (entity is null) return NotFound();
            return View(MapToRow(entity));
        }

        // ──────────────────────────────────────────────────────────────────────
        // POST: /CustomerGroupProductPrices/Delete/5
        // ──────────────────────────────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var entity = await _db.CustomerGroupProductPrices
                .FirstOrDefaultAsync(x => x.GroupPriceId == id);

            if (entity is null) return NotFound();

            _db.CustomerGroupProductPrices.Remove(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã xóa bản ghi giá nhóm.";
            return RedirectToAction(nameof(Index));
        }

        // ══════════════════════════════════════════════════════════════════════
        // POST AJAX: /CustomerGroupProductPrices/ToggleActive
        // ══════════════════════════════════════════════════════════════════════
        [HttpPost]
        public async Task<IActionResult> ToggleActive(long id)
        {
            var entity = await _db.CustomerGroupProductPrices.FindAsync(id);
            if (entity is null) return Json(new { success = false, message = "Không tìm thấy bản ghi." });

            entity.IsActive = !entity.IsActive;
            await _db.SaveChangesAsync();

            return Json(new { success = true, isActive = entity.IsActive });
        }

        // ══════════════════════════════════════════════════════════════════════
        // PRIVATE HELPERS
        // ══════════════════════════════════════════════════════════════════════
        private static CustomerGroupProductPriceRowViewModel MapToRow(CustomerGroupProductPrice e) => new()
        {
            GroupPriceID = e.GroupPriceId,
            CustomerGroupID = e.CustomerGroupId,
            GroupCode = e.CustomerGroup?.GroupCode ?? string.Empty,
            GroupName = e.CustomerGroup?.GroupName ?? string.Empty,
            ProductID = e.ProductId,
            ProductCode = e.Product?.ProductCode ?? string.Empty,
            ProductName = e.Product?.ProductName ?? string.Empty,
            Price = e.Price,
            DiscountRate = e.DiscountRate,
            EffectiveFrom = e.EffectiveFrom,
            EffectiveTo = e.EffectiveTo,
            IsActive = e.IsActive
        };

        private async Task PopulateFormDropdowns(CustomerGroupProductPriceFormViewModel vm)
        {
            vm.CustomerGroupOptions = await _db.CustomerGroups
                .AsNoTracking()
                .Where(g => g.IsActive)
                .OrderBy(g => g.GroupName)
                .Select(g => new SelectListItem(
                    $"{g.GroupCode} – {g.GroupName}",
                    g.CustomerGroupId.ToString()))
                .ToListAsync();

            vm.ProductOptions = await _db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderBy(p => p.ProductName)
                .Select(p => new SelectListItem(
                    $"[{p.ProductCode}] {p.ProductName}",
                    p.ProductId.ToString()))
                .ToListAsync();
        }

        private async Task PopulateFilterDropdowns(CustomerGroupProductPriceFilterViewModel filter)
        {
            ViewBag.CustomerGroupOptions = await _db.CustomerGroups
                .AsNoTracking()
                .Where(g => g.IsActive)
                .OrderBy(g => g.GroupName)
                .Select(g => new SelectListItem(
                    $"{g.GroupCode} – {g.GroupName}",
                    g.CustomerGroupId.ToString(),
                    g.CustomerGroupId == filter.CustomerGroupID))
                .ToListAsync();

            ViewBag.ProductOptions = await _db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderBy(p => p.ProductName)
                .Select(p => new SelectListItem(
                    $"[{p.ProductCode}] {p.ProductName}",
                    p.ProductId.ToString(),
                    p.ProductId == filter.ProductID))
                .ToListAsync();
        }

        private void CustomValidate(CustomerGroupProductPriceFormViewModel vm)
        {
            if (vm.Price == null && vm.DiscountRate == null)
                ModelState.AddModelError(string.Empty,
                    "Phải nhập ít nhất Giá bán riêng hoặc Chiết khấu (%).");

            if (vm.EffectiveTo.HasValue && vm.EffectiveTo < vm.EffectiveFrom)
                ModelState.AddModelError(nameof(vm.EffectiveTo),
                    "Ngày kết thúc phải sau ngày bắt đầu.");
        }

        /// <summary>Kiểm tra khoảng thời gian chồng chéo cho cùng nhóm + sản phẩm.</summary>
        private async Task<bool> HasOverlapAsync(
            long customerGroupID, long productID,
            DateTime from, DateTime? to,
            long? excludeId)
        {
            var query = _db.CustomerGroupProductPrices
                .Where(x => x.CustomerGroupId == customerGroupID && x.ProductId == productID);

            if (excludeId.HasValue)
                query = query.Where(x => x.GroupPriceId != excludeId.Value);

            // hai khoảng [A,B] và [C,D] chồng nhau khi A<=D và C<=B
            return await query.AnyAsync(x =>
                x.EffectiveFrom <= (to ?? DateTime.MaxValue) &&
                (x.EffectiveTo == null || x.EffectiveTo >= from));
        }
    }
}