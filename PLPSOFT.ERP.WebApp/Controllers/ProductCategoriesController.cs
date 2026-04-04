using Microsoft.AspNetCore.Mvc;
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

    // LIST
    public async Task<IActionResult> Index()
    {
        var categories = await _context.ProductCategories
            .Where(c => c.CompanyID == 1)
            .Select(c => new CategoryViewModel
            {
                CategoryID = c.CategoryID,
                CategoryCode = c.CategoryCode,
                CategoryName = c.CategoryName
            })
            .ToListAsync();

        return View(categories);
    }

    // CREATE GET
    public IActionResult Create()
    {
        return View();
    }

    // CREATE POST
    [HttpPost]
    public async Task<IActionResult> Create(ProductCategory model)
    {
        if (ModelState.IsValid)
        {
            model.CompanyID = 1; // 🔥 THÊM DÒNG NÀY

            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // EDIT GET
    // GET
    public async Task<IActionResult> Edit(long id)
    {
        var data = await _context.ProductCategories.FindAsync(id);
        if (data == null) return NotFound();

        var model = new CategoryViewModel
        {
            CategoryID = data.CategoryID,
            CategoryCode = data.CategoryCode,
            CategoryName = data.CategoryName
        };

        return View(model);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var data = await _context.ProductCategories.FindAsync(model.CategoryID);
            if (data == null) return NotFound();

            data.CategoryCode = model.CategoryCode;
            data.CategoryName = model.CategoryName;

            _context.Update(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // DELETE
    public async Task<IActionResult> Delete(long id)
    {
        var data = await _context.ProductCategories.FindAsync(id);
        if (data != null)
        {
            _context.Remove(data);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }


}