using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.WebApp.Models;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly AppDbContext _context;
        public ProductCategoriesController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var categories = await _context.ProductCategories
                .Where(c => c.CompanyID == 1)
                .Select(c => new CategoryViewModel
                {
                    CategoryID = c.CategoryID,
                    CategoryCode = c.CategoryCode,
                    CategoryName = c.CategoryName,
             
                }).ToListAsync();
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new ProductCategory
                {
                    CompanyID = 1,
                    CategoryCode = model.CategoryCode,
                    CategoryName = model.CategoryName,
                    //Description = model.Description,
                    IsActive = true
                };
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}