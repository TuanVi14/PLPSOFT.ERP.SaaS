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

        // GET: CustomerGroupProductPrices
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách giá theo nhóm khách hàng
            return View(await _context.CustomerGroupProductPrice.ToListAsync());
        }

        // GET: CustomerGroupProductPrices/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var customerGroupProductPrice = await _context.CustomerGroupProductPrice
                .FirstOrDefaultAsync(m => m.GroupPriceId == id);

            if (customerGroupProductPrice == null) return NotFound();

            return View(customerGroupProductPrice);
        }

        // GET: CustomerGroupProductPrices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerGroupProductPrices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupPriceId,CustomerGroupId,ProductId,Price,DiscountRate,EffectiveFrom,EffectiveTo,IsActive")] CustomerGroupProductPrice customerGroupProductPrice)
        {
            if (ModelState.IsValid)
            {
                // Thêm mới vào database
                _context.Add(customerGroupProductPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerGroupProductPrice);
        }

        // GET: CustomerGroupProductPrices/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var customerGroupProductPrice = await _context.CustomerGroupProductPrice.FindAsync(id);
            if (customerGroupProductPrice == null) return NotFound();

            return View(customerGroupProductPrice);
        }

        // POST: CustomerGroupProductPrices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("GroupPriceId,CustomerGroupId,ProductId,Price,DiscountRate,EffectiveFrom,EffectiveTo,IsActive")] CustomerGroupProductPrice customerGroupProductPrice)
        {
            if (id != customerGroupProductPrice.GroupPriceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin giá
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
            return View(customerGroupProductPrice);
        }

        // GET: CustomerGroupProductPrices/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var customerGroupProductPrice = await _context.CustomerGroupProductPrice
                .FirstOrDefaultAsync(m => m.GroupPriceId == id);

            if (customerGroupProductPrice == null) return NotFound();

            return View(customerGroupProductPrice);
        }

        // POST: CustomerGroupProductPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customerGroupProductPrice = await _context.CustomerGroupProductPrice.FindAsync(id);
            if (customerGroupProductPrice != null)
            {
                // Xóa khỏi database
                _context.CustomerGroupProductPrice.Remove(customerGroupProductPrice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerGroupProductPriceExists(long id)
        {
            // Kiểm tra tồn tại theo ID
            return _context.CustomerGroupProductPrice.Any(e => e.GroupPriceId == id);
        }
    }
}