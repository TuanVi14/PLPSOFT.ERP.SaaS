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

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var query = _context.Customers
                .Include(c => c.Company)
                .Include(c => c.CustomerGroup)
                .Include(c => c.CustomerType)
                .Where(c => !c.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(c => c.CustomerName.Contains(searchString)
                                      || c.CustomerCode.Contains(searchString)
                                      || (c.Phone != null && c.Phone.Contains(searchString)));
            }

            var list = await query.ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CustomerCode,CustomerName,CustomerTypeId,CustomerGroupId,Phone,Email,IsActive")] Customer vm)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(vm);
            }

            vm.IsActive = true;
            vm.CreatedAt = DateTime.Now;
            _context.Customers.Add(vm);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm khách hàng thành công";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return NotFound();
            LoadDropdowns(entity.CompanyId, entity.CustomerGroupId, entity.CustomerTypeId);
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CustomerId,CompanyId,CustomerCode,CustomerName,CustomerTypeId,CustomerGroupId,Phone,Email,IsActive")] Customer vm)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(vm.CompanyId, vm.CustomerGroupId, vm.CustomerTypeId);
                return View(vm);
            }

            var entity = await _context.Customers.FindAsync(vm.CustomerId);
            if (entity == null) return NotFound();

            entity.CompanyId = vm.CompanyId;
            entity.CustomerCode = vm.CustomerCode;
            entity.CustomerName = vm.CustomerName;
            entity.CustomerTypeId = vm.CustomerTypeId;
            entity.CustomerGroupId = vm.CustomerGroupId;
            entity.Phone = vm.Phone;
            entity.Email = vm.Email;
            entity.IsActive = vm.IsActive;
            entity.UpdatedAt = DateTime.Now;

            _context.Update(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return Json(new { success = false, message = "Không tìm thấy khách hàng!" });

            entity.IsActive = false;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã ngừng hoạt động khách hàng." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(long id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return Json(new { success = false, message = "Không tìm thấy khách hàng!" });

            entity.IsActive = true;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã phục hồi khách hàng." });
        }

        public async Task<IActionResult> Details(long id)
        {
            var entity = await _context.Customers
                .Include(c => c.Company)
                .Include(c => c.CustomerGroup)
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (entity == null) return NotFound();
            return View(entity);
        }

        private void LoadDropdowns(long? companyId = null, long? customerGroupId = null, long? customerTypeId = null)
        {
            ViewBag.Companies = _context.Companies.Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName, Selected = companyId.HasValue && c.CompanyId == companyId.Value });
            ViewBag.CustomerGroups = _context.CustomerGroups.Select(g => new SelectListItem { Value = g.CustomerGroupId.ToString(), Text = g.GroupName, Selected = customerGroupId.HasValue && g.CustomerGroupId == customerGroupId.Value });
            ViewBag.CustomerTypes = _context.SystemTypeValues.Select(t => new SelectListItem { Value = t.TypeValueId.ToString(), Text = t.ValueName, Selected = customerTypeId.HasValue && t.TypeValueId == customerTypeId.Value });
        }
    }
}
