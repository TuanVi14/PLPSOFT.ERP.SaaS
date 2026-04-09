using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // ─── INDEX ───────────────────────────────────────────────────────────
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var query = _context.Customers
                .Include(c => c.Company)
                .Include(c => c.CustomerGroup)
                .Include(c => c.CustomerType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var keyword = searchString.Trim();
                query = query.Where(c =>
                    c.CustomerName.Contains(keyword) ||
                    c.CustomerCode.Contains(keyword) ||
                    (c.Phone != null && c.Phone.Contains(keyword)) ||
                    (c.Email != null && c.Email.Contains(keyword)) ||
                    (c.TaxCode != null && c.TaxCode.Contains(keyword)));
            }

            var list = await query
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(list);
        }

        // ─── CREATE GET ───────────────────────────────────────────────────────
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // ─── CREATE POST ──────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CompanyId,CustomerCode,CustomerName,CustomerTypeId,CustomerGroupId," +
                  "Phone,Email,TaxCode,Gender,DateOfBirth,Facebook,Zalo," +
                  "CreditLimit,PaymentTermDays,IsActive")] Customer vm)
        {
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                    System.Diagnostics.Debug.WriteLine($"❌ [{key}] {error.ErrorMessage}");
            }

            ModelState.Remove(nameof(vm.Company));
            ModelState.Remove(nameof(vm.CustomerGroup));
            ModelState.Remove(nameof(vm.CustomerType));
            ModelState.Remove(nameof(vm.CustomerAddresses));
            ModelState.Remove(nameof(vm.LoyaltyPoint));
            ModelState.Remove(nameof(vm.CreatedAt));
            ModelState.Remove(nameof(vm.IsDeleted));

            // Kiểm tra mã khách hàng trùng
            if (await _context.Customers.AnyAsync(c => c.CustomerCode == vm.CustomerCode && !c.IsDeleted))
                ModelState.AddModelError(nameof(vm.CustomerCode), "Mã khách hàng đã tồn tại.");

            if (!ModelState.IsValid)
            {
                LoadDropdowns(vm.CompanyId, vm.CustomerGroupId, vm.CustomerTypeId);
                return View(vm);
            }

            vm.LoyaltyPoint = 0;
            vm.IsDeleted = false;
            vm.CreatedAt = DateTime.Now;

            _context.Customers.Add(vm);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã thêm khách hàng \"{vm.CustomerName}\" thành công.";
            return RedirectToAction(nameof(Index));
        }

        // ─── DETAILS ──────────────────────────────────────────────────────────
        public async Task<IActionResult> Details(long id)
        {
            var entity = await _context.Customers
                .Include(c => c.Company)
                .Include(c => c.CustomerGroup)
                .Include(c => c.CustomerType)
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);

            if (entity == null) return NotFound();
            return View(entity);
        }

        // ─── EDIT GET ─────────────────────────────────────────────────────────
        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);

            if (entity == null) return NotFound();

            LoadDropdowns(entity.CompanyId, entity.CustomerGroupId, entity.CustomerTypeId);
            return View(entity);
        }

        // ─── EDIT POST ────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("CustomerId,CompanyId,CustomerCode,CustomerName,CustomerTypeId,CustomerGroupId," +
                  "Phone,Email,TaxCode,Gender,DateOfBirth,Facebook,Zalo," +
                  "CreditLimit,PaymentTermDays,IsActive")] Customer vm)
        {

            ModelState.Remove(nameof(vm.Company));
            ModelState.Remove(nameof(vm.CustomerGroup));
            ModelState.Remove(nameof(vm.CustomerType));
            ModelState.Remove(nameof(vm.CustomerAddresses));
            ModelState.Remove(nameof(vm.LoyaltyPoint));
            ModelState.Remove(nameof(vm.UpdatedAt));
            ModelState.Remove(nameof(vm.CreatedAt));
            ModelState.Remove(nameof(vm.IsDeleted));
            // Kiểm tra mã trùng với khách hàng khác
            if (await _context.Customers.AnyAsync(c =>
                    c.CustomerCode == vm.CustomerCode &&
                    c.CustomerId != vm.CustomerId &&
                    !c.IsDeleted))
                ModelState.AddModelError(nameof(vm.CustomerCode), "Mã khách hàng đã tồn tại.");

            if (!ModelState.IsValid)
            {
                LoadDropdowns(vm.CompanyId, vm.CustomerGroupId, vm.CustomerTypeId);
                return View(vm);
            }

            var entity = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == vm.CustomerId && !c.IsDeleted);

            if (entity == null) return NotFound();

            // Chỉ cập nhật các field cho phép — không đụng đến LoyaltyPoint, CreatedAt, IsDeleted, DeletedAt
            entity.CompanyId = vm.CompanyId;
            entity.CustomerCode = vm.CustomerCode;
            entity.CustomerName = vm.CustomerName;
            entity.CustomerTypeId = vm.CustomerTypeId;
            entity.CustomerGroupId = vm.CustomerGroupId;
            entity.Phone = vm.Phone;
            entity.Email = vm.Email;
            entity.TaxCode = vm.TaxCode;
            entity.Gender = vm.Gender;
            entity.DateOfBirth = vm.DateOfBirth;
            entity.Facebook = vm.Facebook;
            entity.Zalo = vm.Zalo;
            entity.CreditLimit = vm.CreditLimit;
            entity.PaymentTermDays = vm.PaymentTermDays;
            entity.IsActive = vm.IsActive;
            entity.UpdatedAt = DateTime.Now;

            _context.Update(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã cập nhật khách hàng \"{entity.CustomerName}\" thành công.";
            return RedirectToAction(nameof(Index));
        }

        // ─── DELETE (soft delete) ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null)
                return Json(new { success = false, message = "Không tìm thấy khách hàng." });

            entity.IsActive = false;
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;

            _context.Update(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã xóa khách hàng \"{entity.CustomerName}\".";
            return RedirectToAction(nameof(Index));
        }

        // ─── RESTORE ──────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(long id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null)
                return Json(new { success = false, message = "Không tìm thấy khách hàng." });

            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.UpdatedAt = DateTime.Now;

            _context.Update(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã khôi phục khách hàng \"{entity.CustomerName}\".";
            return RedirectToAction(nameof(Index));
        }

        // ─── PRIVATE HELPERS ──────────────────────────────────────────────────
        private void LoadDropdowns(
            long? companyId = null,
            long? customerGroupId = null,
            long? customerTypeId = null)
        {
            ViewBag.Companies = _context.Companies
                .Where(c => c.IsActive)
                .OrderBy(c => c.CompanyName)
                .Select(c => new SelectListItem
                {
                    Value = c.CompanyId.ToString(),
                    Text = c.CompanyName,
                    Selected = companyId.HasValue && c.CompanyId == companyId.Value
                });

            ViewBag.CustomerGroups = _context.CustomerGroups
                .Where(g => g.IsActive)
                .OrderBy(g => g.GroupName)
                .Select(g => new SelectListItem
                {
                    Value = g.CustomerGroupId.ToString(),
                    Text = g.GroupName,
                    Selected = customerGroupId.HasValue && g.CustomerGroupId == customerGroupId.Value
                });

            ViewBag.CustomerTypes = _context.SystemTypeValues
                .Where(t => t.IsActive)
                .OrderBy(t => t.ValueName)
                .Select(t => new SelectListItem
                {
                    Value = t.TypeValueId.ToString(),
                    Text = t.ValueName,
                    Selected = customerTypeId.HasValue && t.TypeValueId == customerTypeId.Value
                });

            ViewBag.Genders = new List<SelectListItem>
            {
                new SelectListItem { Value = "",        Text = "-- Chọn giới tính --" },
                new SelectListItem { Value = "Male",    Text = "Nam" },
                new SelectListItem { Value = "Female",  Text = "Nữ" },
                new SelectListItem { Value = "Other",   Text = "Khác" }
            };
        }
    }
}