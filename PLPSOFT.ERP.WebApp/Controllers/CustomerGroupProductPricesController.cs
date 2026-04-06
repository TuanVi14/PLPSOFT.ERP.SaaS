using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.WebApp.Data;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class CustomerGroupProductPricesController : Controller
    {
        private readonly PLPSOFTERPWebAppContext _context;

        public CustomerGroupProductPricesController(PLPSOFTERPWebAppContext context)
        {
            _context = context;
        }

        // 1. SỬA INDEX: Chỉ lấy những dòng chưa bị xóa mềm (IsDelete == false)
        public async Task<IActionResult> Index()
        {
            var list = await _context.CustomerGroupProductPrice
                .Where(x => x.IsDelete == false) // Lọc dữ liệu
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var customerGroupProductPrice = await _context.CustomerGroupProductPrice
                .FirstOrDefaultAsync(m => m.GroupPriceId == id && m.IsDelete == false);

            if (customerGroupProductPrice == null) return NotFound();

            return View(customerGroupProductPrice);
        }

        // 2. SỬA CREATE (GET): Đổ danh sách Tên để chọn lấy ID
        public IActionResult Create()
        {
            // Giả sử bảng Nhóm KH là CustomerGroups và bảng Sản phẩm là Products
            // Bạn hãy thay tên DbSet đúng với thực tế trong DbContext của bạn
            ViewBag.CustomerGroupId = new SelectList(_context.Set<CustomerGroup>().Where(x => !x.IsDelete), "Id", "GroupName");
            ViewBag.ProductId = new SelectList(_context.Set<Product>().Where(x => !x.IsDelete), "Id", "ProductName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupPriceId,CustomerGroupId,ProductId,Price,DiscountRate,EffectiveFrom,EffectiveTo,IsActive")] CustomerGroupProductPrice customerGroupProductPrice)
        {
            if (ModelState.IsValid)
            {
                customerGroupProductPrice.IsDelete = false; // Mặc định chưa xóa
                _context.Add(customerGroupProductPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi, phải đổ lại dữ liệu cho Dropdown
            ViewBag.CustomerGroupId = new SelectList(_context.Set<CustomerGroup>().Where(x => !x.IsDelete), "Id", "GroupName", customerGroupProductPrice.CustomerGroupId);
            ViewBag.ProductId = new SelectList(_context.Set<Product>().Where(x => !x.IsDelete), "Id", "ProductName", customerGroupProductPrice.ProductId);
            return View(customerGroupProductPrice);
        }

        // 3. SỬA EDIT (GET): Đổ dữ liệu vào Dropdown
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var customerGroupProductPrice = await _context.CustomerGroupProductPrice.FindAsync(id);
            if (customerGroupProductPrice == null || customerGroupProductPrice.IsDelete == true) return NotFound();

            ViewBag.CustomerGroupId = new SelectList(_context.Set<CustomerGroup>().Where(x => !x.IsDelete), "Id", "GroupName", customerGroupProductPrice.CustomerGroupId);
            ViewBag.ProductId = new SelectList(_context.Set<Product>().Where(x => !x.IsDelete), "Id", "ProductName", customerGroupProductPrice.ProductId);

            return View(customerGroupProductPrice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("GroupPriceId,CustomerGroupId,ProductId,Price,DiscountRate,EffectiveFrom,EffectiveTo,IsActive")] CustomerGroupProductPrice customerGroupProductPrice)
        {
            if (id != customerGroupProductPrice.GroupPriceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerGroupProductPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerGroupProductPriceExists(customerGroupProductPrice.GroupPriceId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CustomerGroupId = new SelectList(_context.Set<CustomerGroup>().Where(x => !x.IsDelete), "Id", "GroupName", customerGroupProductPrice.CustomerGroupId);
            ViewBag.ProductId = new SelectList(_context.Set<Product>().Where(x => !x.IsDelete), "Id", "ProductName", customerGroupProductPrice.ProductId);
            return View(customerGroupProductPrice);
        }

        // 4. SỬA DELETE: Chuyển thành Xóa Mềm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var item = await _context.CustomerGroupProductPrice.FindAsync(id);
            if (item != null)
            {
                // KHÔNG dùng _context.Remove
                item.IsDelete = true;  // Đánh dấu xóa
                item.IsActive = false; // Ngừng hoạt động
                _context.Update(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerGroupProductPriceExists(long id)
        {
            return _context.CustomerGroupProductPrice.Any(e => e.GroupPriceId == id && e.IsDelete == false);
        }
    }
}