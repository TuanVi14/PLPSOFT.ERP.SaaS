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
                Category = p.Category,

            }).ToList();


            // 3. Trả về View với kiểu dữ liệu đã khớp (ProductViewModel)
            return View(productViewModels);
        }

        // GET: Thêm sản phẩm
        public IActionResult Create(long? companyId)
        {
            var vm = new ProductViewModel
            {
                CompanyID = companyId ?? 0
            };

            LoadDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel vm)
        {
            if (vm.CompanyID == 0)
            {
                ModelState.AddModelError("CompanyID", "Vui lòng chọn công ty");
            }
            if (!ModelState.IsValid)
            {
                LoadDropdowns(vm); 
                return View(vm);
            }

            var exists = await _context.Products.AnyAsync(p =>
                    p.CompanyId == vm.CompanyID &&
                    p.ProductCode == vm.ProductCode);

            if (exists)
            {
                ModelState.AddModelError(nameof(vm.ProductCode),
                    "Mã sản phẩm đã tồn tại trong công ty này.");
                return View(vm);
            }

            var product = new Product
            {
                CompanyId = vm.CompanyID,
                ProductCode = vm.ProductCode,
                ProductName = vm.ProductName,
                CategoryId = vm.CategoryID,
                ProductTypeId = vm.ProductTypeID,
                BaseUnitId = vm.BaseUnitID,

                // 🔥 NEW
                Sku = vm.Sku,
                Barcode = vm.Barcode,
                Brand = vm.Brand,
                Origin = vm.Origin,

                CostPrice = vm.CostPrice,
                StandardPrice = vm.StandardPrice,
                DefaultTaxRateId = vm.DefaultTaxRateID,

                TrackInventory = vm.TrackInventory,
                AllowBackorder = vm.AllowBackorder,
                Weight = vm.Weight,
                Volume = vm.Volume,
                MinStock = vm.MinStock,
                MaxStock = vm.MaxStock,

                WarrantyMonths = vm.WarrantyMonths,
                IsSerialized = vm.IsSerialized,
                IsBatchManaged = vm.IsBatchManaged,
                ExpireDateRequired = vm.ExpireDateRequired,

                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Sửa sản phẩm
        public async Task<IActionResult> Edit(long id, long? companyId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                ProductID = product.ProductId,
                CompanyID = companyId ?? product.CompanyId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                CategoryID = product.CategoryId,
                ProductTypeID = product.ProductTypeId,
                BaseUnitID = product.BaseUnitId,

                // 🔥 NEW
                Sku = product.Sku,
                Barcode = product.Barcode,
                Brand = product.Brand,
                Origin = product.Origin,

                CostPrice = product.CostPrice,
                StandardPrice = product.StandardPrice,
                DefaultTaxRateID = product.DefaultTaxRateId,

                TrackInventory = product.TrackInventory,
                AllowBackorder = product.AllowBackorder,
                Weight = product.Weight,
                Volume = product.Volume,
                MinStock = product.MinStock,
                MaxStock = product.MaxStock,

                WarrantyMonths = product.WarrantyMonths,
                IsSerialized = product.IsSerialized,
                IsBatchManaged = product.IsBatchManaged,
                ExpireDateRequired = product.ExpireDateRequired,

                IsActive = product.IsActive
            };

            LoadDropdowns(vm);
            return View(vm);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(vm);
                return View(vm);
            }

            var product = await _context.Products.FindAsync(vm.ProductID);
            if (product == null) return NotFound();

            product.CompanyId = vm.CompanyID;
            product.ProductName = vm.ProductName;
            product.CategoryId = vm.CategoryID;
            product.ProductTypeId = vm.ProductTypeID;
            product.BaseUnitId = vm.BaseUnitID;

            // 🔥 NEW
            product.Sku = vm.Sku;
            product.Barcode = vm.Barcode;
            product.Brand = vm.Brand;
            product.Origin = vm.Origin;

            product.CostPrice = vm.CostPrice;
            product.StandardPrice = vm.StandardPrice;
            product.DefaultTaxRateId = vm.DefaultTaxRateID;

            product.TrackInventory = vm.TrackInventory;
            product.AllowBackorder = vm.AllowBackorder;
            product.Weight = vm.Weight;
            product.Volume = vm.Volume;
            product.MinStock = vm.MinStock;
            product.MaxStock = vm.MaxStock;

            product.WarrantyMonths = vm.WarrantyMonths;
            product.IsSerialized = vm.IsSerialized;
            product.IsBatchManaged = vm.IsBatchManaged;
            product.ExpireDateRequired = vm.ExpireDateRequired;

            product.IsActive = vm.IsActive;
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Details(long id)
        {
            var product = await _context.Products
                .Include(p => p.Company)
                .Include(p => p.Category)
                .Include(p => p.BaseUnit)
                .Include(p => p.DefaultTaxRate)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }
        private void LoadDropdowns(ProductViewModel vm)
        {
            vm.Companies = _context.Companies
                .Select(c => new SelectListItem
                {
                    Value = c.CompanyId.ToString(),
                    Text = c.CompanyName
                });

            vm.Categories = _context.ProductCategories
                .Where(c => c.CompanyId == vm.CompanyID)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                });

            vm.ProductTypes = _context.SystemTypeValues
                .Where(x => x.TypeId == 4)
                .Select(x => new SelectListItem
                {
                    Value = x.TypeValueId.ToString(),
                    Text = x.ValueName
                });

            vm.Units = _context.ProductUnits
                .Select(u => new SelectListItem
                {
                    Value = u.UnitId.ToString(),
                    Text = u.UnitName
                });

            vm.TaxRates = _context.TaxRates
                .Where(t => vm.CompanyID == 0 || t.CompanyId == vm.CompanyID)
                .Select(t => new SelectListItem
                {
                    Value = t.TaxRateId.ToString(),
                    Text = t.TaxName
                });
        }
    }
}