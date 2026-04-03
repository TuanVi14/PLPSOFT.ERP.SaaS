using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Sales.SaaS.V2026.Data;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class CustomerAddressesController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerAddressesController(AppDbContext context)
        {
            _context = context;
        }

        // Chỉnh sửa Index để lọc theo customerId
        public async Task<IActionResult> Index(string customerId)
        {
            if (string.IsNullOrEmpty(customerId)) return NotFound();

            ViewBag.CustomerId = customerId;
            var addresses = await _context.CustomerAddresses
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();

            return View(addresses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var customerAddress = await _context.CustomerAddresses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customerAddress == null) return NotFound();

            return View(customerAddress);
        }

        // Chỉnh sửa Create để nhận customerId từ màn hình trước
        public IActionResult Create(string customerId)
        {
            return View(new CustomerAddress { CustomerId = customerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,AddressLine,Ward,District,City,IsDefault")] CustomerAddress customerAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerAddress);
                await _context.SaveChangesAsync();
                // Quay về danh sách địa chỉ của đúng khách hàng đó
                return RedirectToAction(nameof(Index), new { customerId = customerAddress.CustomerId });
            }
            return View(customerAddress);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            if (customerAddress == null) return NotFound();

            return View(customerAddress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,AddressLine,Ward,District,City,IsDefault")] CustomerAddress customerAddress)
        {
            if (id != customerAddress.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAddressExists(customerAddress.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index), new { customerId = customerAddress.CustomerId });
            }
            return View(customerAddress);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customerAddress = await _context.CustomerAddresses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customerAddress == null) return NotFound();

            return View(customerAddress);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            string customerId = customerAddress?.CustomerId;

            if (customerAddress != null)
            {
                _context.CustomerAddresses.Remove(customerAddress);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { customerId = customerId });
        }

        private bool CustomerAddressExists(int id)
        {
            return _context.CustomerAddresses.Any(e => e.Id == id);
        }
    }
}