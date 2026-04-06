using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.WebApp.Models; // Thêm để dùng ViewModel

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách sản phẩm
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            // 1. Lấy dữ liệu từ database (kiểu Product)
            var query = _context.Products
                .Include(p => p.Company)
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(p => p.ProductName.Contains(searchString)
                                      || p.ProductCode.Contains(searchString));
            }

            var products = await query.ToListAsync();
            // 2. Chuyển đổi sang List<ProductViewModel>
            var productViewModels = products.Select(p => new ProductViewModel
            {
                ProductID = p.ProductId,
                ProductCode = p.ProductCode,
                ProductName = p.ProductName,
                CostPrice = p.CostPrice,
                StandardPrice = p.StandardPrice,
                IsActive = p.IsActive,
                // Gán object để View Index hiển thị được tên công ty/danh mục
                Company = p.Company,
                Category = p.Category
            }).ToList();


            // 3. Trả về View với kiểu dữ liệu đã khớp (ProductViewModel)
            return View(productViewModels);
        }

        // GET: Thêm sản phẩm
        public IActionResult Create()
        {
            var vm = new ProductViewModel
            {
                Companies = _context.Companies.Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName }),
                Categories = _context.ProductCategories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName }),
                ProductTypes = _context.SystemTypeValues.Where(x => x.TypeId == 4).Select(x => new SelectListItem { Value = x.TypeValueId.ToString(), Text = x.ValueName }),
                Units = _context.ProductUnits.Select(u => new SelectListItem { Value = u.UnitId.ToString(), Text = u.UnitName })
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    CompanyId = vm.CompanyID,
                    ProductCode = vm.ProductCode,
                    ProductName = vm.ProductName,
                    CategoryId = vm.CategoryID,
                    ProductTypeId = vm.ProductTypeID,
                    BaseUnitId = vm.BaseUnitID,
                    CostPrice = vm.CostPrice,
                    StandardPrice = vm.StandardPrice,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Sửa sản phẩm
        public async Task<IActionResult> Edit(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                ProductID = product.ProductId,
                CompanyID = product.CompanyId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                CategoryID = product.CategoryId,
                ProductTypeID = product.ProductTypeId,
                BaseUnitID = product.BaseUnitId,
                CostPrice = product.CostPrice,
                StandardPrice = product.StandardPrice,
                IsActive = product.IsActive,

                // QUAN TRỌNG: Phải khởi tạo các List này, nếu không View sẽ bị NullReferenceException
                Companies = _context.Companies.Select(c => new SelectListItem
                {
                    Value = c.CompanyId.ToString(),
                    Text = c.CompanyName
                }).ToList(),
                Categories = _context.ProductCategories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList(),
                ProductTypes = _context.SystemTypeValues.Where(v => v.TypeId == 4).Select(v => new SelectListItem
                {
                    Value = v.TypeValueId.ToString(),
                    Text = v.ValueName
                }).ToList(),
                Units = _context.ProductUnits.Select(u => new SelectListItem
                {
                    Value = u.UnitId.ToString(),
                    Text = u.UnitName
                }).ToList()
            };

            return View(vm);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(vm.ProductID);
                if (product == null) return NotFound();

                product.CompanyId = vm.CompanyID;
                product.ProductName = vm.ProductName;
                product.CategoryId = vm.CategoryID;
                product.ProductTypeId = vm.ProductTypeID;
                product.BaseUnitId = vm.BaseUnitID;
                product.CostPrice = vm.CostPrice;
                product.StandardPrice = vm.StandardPrice;
                product.IsActive = vm.IsActive;

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // NẾU VALIDATE THẤT BẠI: Bạn cũng phải nạp lại các List này trước khi return View(vm)
            // Nếu thiếu đoạn này, khi nhấn "Lưu" mà bị lỗi nhập liệu, trang web sẽ văng lỗi NullReferenceException ngay.
            vm.Companies = _context.Companies.Select(c => new SelectListItem { Value = c.CompanyId.ToString(), Text = c.CompanyName }).ToList();
            vm.Categories = _context.ProductCategories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName }).ToList();
            vm.ProductTypes = _context.SystemTypeValues.Where(v => v.TypeId == 4).Select(v => new SelectListItem { Value = v.TypeValueId.ToString(), Text = v.ValueName }).ToList();
            vm.Units = _context.ProductUnits.Select(u => new SelectListItem { Value = u.UnitId.ToString(), Text = u.UnitName }).ToList();

            return View(vm);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });

            // Thay vì xóa vĩnh viễn, chỉ chuyển trạng thái
            product.IsActive = false;

            _context.Update(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã chuyển sản phẩm sang trạng thái 'Ngừng hoạt động'." });
        }

        // POST: Product/Restore/5
        [HttpPost]
        public async Task<IActionResult> Restore(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });

            product.IsActive = true; // Phục hồi trạng thái

            _context.Update(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Sản phẩm đã được kích hoạt trở lại." });
        }
    }
}