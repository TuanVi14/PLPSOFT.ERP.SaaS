using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.WebApp.Models;

public class CustomerGroupsController : Controller
{
    private readonly AppDbContext _context;

    public CustomerGroupsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var query = _context.CustomerGroups.Include(c => c.Company).AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.Trim();
            query = query.Where(c => c.GroupName.Contains(searchString) || c.GroupCode.Contains(searchString));
        }
        var list = await query.ToListAsync();
        return View(list);
    }

    public IActionResult Create()
    {
        ViewBag.Companies = _context.Companies.Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName });
        return View(new CustomerGroup { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyId,GroupCode,GroupName,Description,IsActive")] CustomerGroup vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Companies = _context.Companies.Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName });
            return View(vm);
        }

        _context.CustomerGroups.Add(vm);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id)
    {
        var entity = await _context.CustomerGroups.FindAsync(id);
        if (entity == null) return NotFound();

        ViewBag.Companies = _context.Companies.Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName, Selected = c.CompanyId == entity.CompanyId });
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("CustomerGroupId,CompanyId,GroupCode,GroupName,Description,IsActive")] CustomerGroup vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Companies = _context.Companies.Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName, Selected = c.CompanyId == vm.CompanyId });
            return View(vm);
        }

        var entity = await _context.CustomerGroups.FindAsync(vm.CustomerGroupId);
        if (entity == null) return NotFound();

        entity.CompanyId = vm.CompanyId;
        entity.GroupCode = vm.GroupCode;
        entity.GroupName = vm.GroupName;
        entity.Description = vm.Description;
        entity.IsActive = vm.IsActive;

        _context.Update(entity);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _context.CustomerGroups.FindAsync(id);
        if (entity == null) return Json(new { success = false, message = "Không tìm thấy nhóm khách hàng!" });

        entity.IsActive = false;
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return Json(new { success = true, message = "Đã ngừng hoạt động nhóm khách hàng." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(long id)
    {
        var entity = await _context.CustomerGroups.FindAsync(id);
        if (entity == null) return Json(new { success = false, message = "Không tìm thấy nhóm khách hàng!" });

        entity.IsActive = true;
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return Json(new { success = true, message = "Đã phục hồi nhóm khách hàng." });
    }
}
