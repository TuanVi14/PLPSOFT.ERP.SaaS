using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.WebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductUnitMappingController : Controller
    {
        private readonly AppDbContext _db;

        public ProductUnitMappingController(AppDbContext db) => _db = db;

        // ══════════════════════════════════════════════════════════════════════
        // GET: /ProductUnitMappings?productId=xxx
        // Hiển thị danh sách đơn vị tính của một sản phẩm
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Index(long? productId, string? keyword, int page = 1)
        {
            const int pageSize = 20;

            // Nếu không truyền productId → hiển thị toàn bộ, group theo sản phẩm
            var mappingQuery = _db.ProductUnitMappings
                .AsNoTracking()
                .Include(m => m.Product)
                .Include(m => m.Unit)
                .AsQueryable();

            if (productId.HasValue)
                mappingQuery = mappingQuery.Where(m => m.ProductId == productId.Value);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                mappingQuery = mappingQuery.Where(m =>
                    m.Product.ProductCode.ToLower().Contains(kw) ||
                    m.Product.ProductName.ToLower().Contains(kw) ||
                    m.Unit.UnitCode.ToLower().Contains(kw) ||
                    m.Unit.UnitName.ToLower().Contains(kw));
            }

            var totalCount = await mappingQuery.CountAsync();

            var rows = await mappingQuery
                .OrderBy(m => m.Product.ProductName)
                .ThenByDescending(m => m.IsDefault)
                .ThenBy(m => m.Unit.UnitName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new ProductUnitMappingRowViewModel
                {
                    ProductID = m.ProductId,
                    ProductName = m.Product.ProductName,
                    UnitID = m.UnitId,
                    UnitCode = m.Unit.UnitCode,
                    UnitName = m.Unit.UnitName,
                    ConversionRate = m.ConversionRate,
                    IsDefault = m.IsDefault
                })
                .ToListAsync();

            // Lấy thêm thông tin sản phẩm cho header khi lọc theo 1 sản phẩm
            ProductUnitMappingListViewModel vm;

            if (productId.HasValue)
            {
                var product = await _db.Products
                    .AsNoTracking()
                    .Include(p => p.BaseUnit)
                    .Where(p => p.ProductId == productId.Value)
                    .Select(p => new { p.ProductId, p.ProductCode, p.ProductName, BaseUnitName = p.BaseUnit.UnitName })
                    .FirstOrDefaultAsync();

                vm = new ProductUnitMappingListViewModel
                {
                    ProductID = product?.ProductId ?? 0,
                    ProductCode = product?.ProductCode ?? string.Empty,
                    ProductName = product?.ProductName ?? string.Empty,
                    BaseUnitName = product?.BaseUnitName ?? string.Empty,
                    Mappings = rows
                };
            }
            else
            {
                vm = new ProductUnitMappingListViewModel { Mappings = rows };
            }

            ViewBag.TotalCount = totalCount;
            ViewBag.PageIndex = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);
            ViewBag.ProductId = productId;
            ViewBag.Keyword = keyword;

            // Dropdown sản phẩm cho bộ lọc
            ViewBag.ProductOptions = await _db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderBy(p => p.ProductName)
                .Select(p => new SelectListItem(
                    $"[{p.ProductCode}] {p.ProductName}",
                    p.ProductId.ToString(),
                    p.ProductId == productId))
                .ToListAsync();

            return View(vm);
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: /ProductUnitMappings/Create?productId=xxx
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Create(long? productId)
        {
            var vm = new ProductUnitMappingFormViewModel
            {
                ProductID = productId ?? 0
            };
            await PopulateFormDropdowns(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────────────────────────────
        // POST: /ProductUnitMappings/Create
        // ──────────────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductUnitMappingFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateFormDropdowns(vm);
                return View(vm);
            }

            // Kiểm tra khóa chính đã tồn tại chưa
            var exists = await _db.ProductUnitMappings
                .AnyAsync(m => m.ProductId == vm.ProductID && m.UnitId == vm.UnitID);

            if (exists)
            {
                ModelState.AddModelError(string.Empty,
                    "Đơn vị tính này đã được cấu hình cho sản phẩm trên rồi.");
                await PopulateFormDropdowns(vm);
                return View(vm);
            }

            // Nếu đặt IsDefault = true → bỏ IsDefault của các mapping khác của sản phẩm này
            if (vm.IsDefault)
                await ClearDefaultAsync(vm.ProductID, null);

            var entity = new ProductUnitMapping
            {
                ProductId = vm.ProductID,
                UnitId = vm.UnitID,
                ConversionRate = vm.ConversionRate,
                IsDefault = vm.IsDefault
            };

            _db.ProductUnitMappings.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Thêm đơn vị tính thành công!";
            return RedirectToAction(nameof(Index), new { productId = vm.ProductID });
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: /ProductUnitMappings/Edit?productId=1&unitId=2
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Edit(long productId, long unitId)
        {
            var entity = await _db.ProductUnitMappings
                .AsNoTracking()
                .Include(m => m.Product)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.ProductId == productId && m.UnitId == unitId);

            if (entity is null) return NotFound();

            var vm = new ProductUnitMappingFormViewModel
            {
                OriginalProductID = entity.ProductId,
                OriginalUnitID = entity.UnitId,
                ProductID = entity.ProductId,
                UnitID = entity.UnitId,
                ConversionRate = entity.ConversionRate,
                IsDefault = entity.IsDefault,
                ProductDisplayName = $"[{entity.Product?.ProductCode}] {entity.Product?.ProductName}",
                UnitDisplayName = $"{entity.Unit?.UnitCode} – {entity.Unit?.UnitName}"
            };

            await PopulateFormDropdowns(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────────────────────────────
        // POST: /ProductUnitMappings/Edit
        // ──────────────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUnitMappingFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateFormDropdowns(vm);
                return View(vm);
            }

            var entity = await _db.ProductUnitMappings
                .FirstOrDefaultAsync(m =>
                    m.ProductId == vm.OriginalProductID &&
                    m.UnitId == vm.OriginalUnitID);

            if (entity is null) return NotFound();

            // Đơn vị tính thay đổi → kiểm tra PK mới không trùng
            if (vm.UnitID != vm.OriginalUnitID)
            {
                var newExists = await _db.ProductUnitMappings
                    .AnyAsync(m => m.ProductId == vm.ProductID && m.UnitId == vm.UnitID);

                if (newExists)
                {
                    ModelState.AddModelError(nameof(vm.UnitID),
                        "Đơn vị tính này đã tồn tại cho sản phẩm trên.");
                    await PopulateFormDropdowns(vm);
                    return View(vm);
                }

                // PK ghép không thể UPDATE → xóa rồi thêm mới
                _db.ProductUnitMappings.Remove(entity);
                await _db.SaveChangesAsync();

                if (vm.IsDefault) await ClearDefaultAsync(vm.ProductID, null);

                _db.ProductUnitMappings.Add(new ProductUnitMapping
                {
                    ProductId = vm.ProductID,
                    UnitId = vm.UnitID,
                    ConversionRate = vm.ConversionRate,
                    IsDefault = vm.IsDefault
                });
            }
            else
            {
                if (vm.IsDefault) await ClearDefaultAsync(vm.ProductID, vm.UnitID);
                entity.ConversionRate = vm.ConversionRate;
                entity.IsDefault = vm.IsDefault;
            }

            await _db.SaveChangesAsync();

            TempData["Success"] = "Cập nhật đơn vị tính thành công!";
            return RedirectToAction(nameof(Index), new { productId = vm.ProductID });
        }

        // ══════════════════════════════════════════════════════════════════════
        // GET: /ProductUnitMappings/Delete?productId=1&unitId=2
        // ══════════════════════════════════════════════════════════════════════
        public async Task<IActionResult> Delete(long productId, long unitId)
        {
            var entity = await _db.ProductUnitMappings
                .AsNoTracking()
                .Include(m => m.Product)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.ProductId == productId && m.UnitId == unitId);

            if (entity is null) return NotFound();

            var vm = new ProductUnitMappingRowViewModel
            {
                ProductID = entity.ProductId,
                UnitID = entity.UnitId,
                UnitCode = entity.Unit?.UnitCode ?? string.Empty,
                UnitName = entity.Unit?.UnitName ?? string.Empty,
                ConversionRate = entity.ConversionRate,
                IsDefault = entity.IsDefault
            };

            ViewBag.ProductCode = entity.Product?.ProductCode;
            ViewBag.ProductName = entity.Product?.ProductName;
            return View(vm);
        }

        // ──────────────────────────────────────────────────────────────────────
        // POST: /ProductUnitMappings/Delete
        // ──────────────────────────────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long productId, long unitId)
        {
            var entity = await _db.ProductUnitMappings
                .FirstOrDefaultAsync(m => m.ProductId == productId && m.UnitId == unitId);

            if (entity is null) return NotFound();

            if (entity.IsDefault)
            {
                TempData["Error"] = "Không thể xóa đơn vị tính mặc định. Hãy đặt đơn vị khác làm mặc định trước.";
                return RedirectToAction(nameof(Index), new { productId });
            }

            _db.ProductUnitMappings.Remove(entity);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã xóa đơn vị tính.";
            return RedirectToAction(nameof(Index), new { productId });
        }

        // ══════════════════════════════════════════════════════════════════════
        // POST AJAX: /ProductUnitMappings/SetDefault?productId=1&unitId=2
        // ══════════════════════════════════════════════════════════════════════
        [HttpPost]
        public async Task<IActionResult> SetDefault(long productId, long unitId)
        {
            var entity = await _db.ProductUnitMappings
                .FirstOrDefaultAsync(m => m.ProductId == productId && m.UnitId == unitId);

            if (entity is null)
                return Json(new { success = false, message = "Không tìm thấy." });

            await ClearDefaultAsync(productId, unitId);
            entity.IsDefault = true;
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ══════════════════════════════════════════════════════════════════════
        // PRIVATE HELPERS
        // ══════════════════════════════════════════════════════════════════════
        private async Task PopulateFormDropdowns(ProductUnitMappingFormViewModel vm)
        {
            vm.ProductOptions = await _db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderBy(p => p.ProductName)
                .Select(p => new SelectListItem(
                    $"[{p.ProductCode}] {p.ProductName}",
                    p.ProductId.ToString()))
                .ToListAsync();

            vm.UnitOptions = await _db.ProductUnits
                .AsNoTracking()
                .Where(u => u.IsActive)
                .OrderBy(u => u.UnitName)
                .Select(u => new SelectListItem(
                    $"{u.UnitCode} – {u.UnitName}",
                    u.UnitId.ToString()))
                .ToListAsync();
        }

        /// <summary>Bỏ IsDefault của tất cả mapping của productId, trừ excludeUnitId.</summary>
        private async Task ClearDefaultAsync(long productId, long? excludeUnitId)
        {
            var others = await _db.ProductUnitMappings
                .Where(m => m.ProductId == productId &&
                            (excludeUnitId == null || m.UnitId != excludeUnitId) &&
                            m.IsDefault)
                .ToListAsync();

            foreach (var o in others) o.IsDefault = false;
            // SaveChanges được gọi ở caller
        }
    }
}