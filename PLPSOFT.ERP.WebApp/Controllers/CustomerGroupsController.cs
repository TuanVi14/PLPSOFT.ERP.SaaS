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
    public async Task<IActionResult> Create(
    [Bind("CompanyID,GroupCode,GroupName,Description,IsActive")] CustomerGroupViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Companies = _context.Companies.Select(c => new SelectListItem
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyName
            });
            return View(vm);
        }

        var entity = new CustomerGroup
        {
            CompanyId = vm.CompanyID,
            GroupCode = vm.GroupCode,
            GroupName = vm.GroupName,
            Description = vm.Description,
            IsActive = vm.IsActive
        };

        try
        {
            _context.CustomerGroups.Add(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
            vm.Companies = _context.Companies.Select(c => new SelectListItem
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyName
            });
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(long id)
    {
        var entity = await _context.CustomerGroups.FindAsync(id);
        if (entity == null) return NotFound();

        var vm = new CustomerGroupViewModel
        {
            CustomerGroupID = entity.CustomerGroupId,
            CompanyID = entity.CompanyId,
            GroupCode = entity.GroupCode,
            GroupName = entity.GroupName,
            Description = entity.Description,
            IsActive = entity.IsActive,
            Companies = _context.Companies.Select(c => new SelectListItem
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyName,
                Selected = c.CompanyId == entity.CompanyId
            })
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    [Bind("CustomerGroupID,GroupCode,GroupName,Description,IsActive")] CustomerGroupViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var current = await _context.CustomerGroups.FindAsync(vm.CustomerGroupID);
            vm.Companies = _context.Companies.Select(c => new SelectListItem
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyName,
                Selected = c.CompanyId == current!.CompanyId
            });
            return View(vm);
        }

        var entity = await _context.CustomerGroups.FindAsync(vm.CustomerGroupID);
        if (entity == null) return NotFound();

        // CompanyId giữ nguyên từ entity, không cập nhật
        entity.GroupCode = vm.GroupCode;
        entity.GroupName = vm.GroupName;
        entity.Description = vm.Description;
        entity.IsActive = vm.IsActive;

        try
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
            vm.Companies = _context.Companies.Select(c => new SelectListItem
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyName,
                Selected = c.CompanyId == entity.CompanyId
            });
            return View(vm);
        }
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
        return RedirectToAction(nameof(Index));
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
        return RedirectToAction(nameof(Index));
    }
}
