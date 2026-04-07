using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.WebApp.Models;

public class ProductCategoriesController : Controller
{
    private readonly AppDbContext _context;

    public ProductCategoriesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var query = _context.ProductCategories
               .Include(p => p.Company)
               .Include(p => p.Parent)
               .AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.Trim();
            query = query.Where(p => p.CategoryName.Contains(searchString)
                                  || p.CategoryCode.Contains(searchString));
        }
        var categories = await query.ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        var vm = new CategoryViewModel
        {
            Companies = _context.Companies
                .Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName }),
            ParentCategories = _context.ProductCategories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var category = new ProductCategory
            {
                CompanyId = vm.CompanyID,
                ParentId = vm.ParentID,
                CategoryCode = vm.CategoryCode,
                CategoryName = vm.CategoryName,
                IsActive = vm.IsActive,
            };
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Companies = _context.Companies
            .Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName });
        vm.ParentCategories = _context.ProductCategories
            .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName });
        return View(vm);
    }

    public async Task<IActionResult> Edit(long id)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null) return NotFound();

        var vm = new CategoryViewModel
        {
            CategoryID = category.CategoryId,
            CompanyID = category.CompanyId,
            ParentID = category.ParentId,
            CategoryCode = category.CategoryCode,
            CategoryName = category.CategoryName,
            IsActive = category.IsActive,
            Companies = _context.Companies
                .Select(c => new SelectListItem
                {
                    Value = c.CompanyId.ToString(),
                    Text = c.CompanyName,
                    Selected = c.CompanyId == category.CompanyId
                }),
            ParentCategories = _context.ProductCategories
                .Where(c => c.CategoryId != id)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName,
                    Selected = c.CategoryId == category.ParentId
                })
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var category = await _context.ProductCategories.FindAsync(vm.CategoryID);
            if (category == null) return NotFound();

            category.CompanyId = vm.CompanyID;
            category.ParentId = vm.ParentID;
            category.CategoryCode = vm.CategoryCode;
            category.CategoryName = vm.CategoryName;
            category.IsActive = vm.IsActive;

            _context.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    // POST: ProductCategories/Delete/13
    [HttpPost]
    [ValidateAntiForgeryToken] // Đảm bảo có dòng này để bảo mật
    public async Task<IActionResult> Delete(long id) // LUÔN ĐẶT TÊN LÀ 'id'
    {
        var category = await _context.ProductCategories
            .Include(c => c.InverseParent)
            .FirstOrDefaultAsync(m => m.CategoryId == id);

        if (category == null) return Json(new { success = false, message = "Không tìm thấy!" });

        if (category.InverseParent.Any(c => c.IsActive))
        {
            return Json(new { success = false, message = "Danh mục này có danh mục con đang hoạt động, không thể ngừng dùng!" });
        }

        category.IsActive = false;
        _context.Update(category);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Đã ngừng hoạt động danh mục." });
    }

    // POST: ProductCategories/Restore/13
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(long id) // ĐẶT TÊN LÀ 'id'
    {
        var category = await _context.ProductCategories
            .Include(c => c.Parent)
            .FirstOrDefaultAsync(m => m.CategoryId == id);

        if (category == null) return Json(new { success = false, message = "Không tìm thấy!" });

        if (category.Parent != null && !category.Parent.IsActive)
        {
            return Json(new { success = false, message = $"Phải phục hồi danh mục cha [{category.Parent.CategoryName}] trước!" });
        }

        category.IsActive = true;
        _context.Update(category);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Đã phục hồi danh mục." });
    }
}

