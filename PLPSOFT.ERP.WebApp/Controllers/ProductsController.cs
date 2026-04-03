using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.Domain.Entities;
using PLPSOFT.ERP.WebApp.Models; // Thêm để dùng ViewModel

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. DANH SÁCH (Dùng ViewModel)
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .Where(p => p.CompanyID == 1);

            // 🔍 THÊM ĐOẠN NÀY
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p =>
                    p.ProductCode.Contains(searchString) ||
                    p.ProductName.Contains(searchString)
                );
            }

            var products = await query
                .Select(p => new ProductViewModel
                {
                    ProductID = p.ProductID,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    CategoryName = p.Category != null ? p.Category.CategoryName : "Chưa phân loại",
                    UnitName = p.Unit != null ? p.Unit.UnitName : "Chưa có đơn vị",
                    StandardPrice = p.StandardPrice
                })
                .ToListAsync();

            // giữ lại text đã nhập
            ViewBag.SearchString = searchString;

            return View(products);
        }

        // Hàm bổ trợ load Dropdown - Đảm bảo tên ViewBag khớp hoàn toàn với View
        private void PopulateDropdowns(long? selectedCategory = null, long? selectedUnit = null)
        {
            ViewBag.CategoryID = new SelectList(
                _context.ProductCategories.ToList(), // ⚠️ thêm ToList()
                "CategoryID",
                "CategoryName",
                selectedCategory
            );

            ViewBag.BaseUnitID = new SelectList(
                _context.ProductUnits.ToList(), // ⚠️ thêm ToList()
                "ProductUnitID",
                "UnitName",
                selectedUnit
            );
        }

        // 2. THÊM MỚI (GET)
        public IActionResult Create()
        {
            // Gọi hàm này để nạp dữ liệu vào ViewBag
            PopulateDropdowns();
            return View();
        }



        // 3. THÊM MỚI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    CompanyID = 1,
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    CategoryID = model.CategoryID,
                    BaseUnitID = model.BaseUnitID,
                    StandardPrice = model.StandardPrice
                };
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns();
            return View(model);
        }

        // 4. CHỈNH SỬA (GET)
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();

            var model = new ProductViewModel
            {
                ProductID = p.ProductID,
                ProductCode = p.ProductCode,
                ProductName = p.ProductName,
                CategoryID = p.CategoryID,
                BaseUnitID = p.BaseUnitID,
                StandardPrice = p.StandardPrice
            };

            PopulateDropdowns(p.CategoryID, p.BaseUnitID);
            return View(model);
        }

        // 5. CHỈNH SỬA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ProductViewModel model)
        {
            if (id != model.ProductID) return NotFound();

            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return NotFound();

                product.ProductName = model.ProductName;
                product.ProductCode = model.ProductCode;
                product.CategoryID = model.CategoryID;
                product.BaseUnitID = model.BaseUnitID;
                product.StandardPrice = model.StandardPrice;

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(model.CategoryID, model.BaseUnitID);
            return View(model);
        }

        // 6. XÓA (Thực hiện nhanh)
        public async Task<IActionResult> Delete(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}