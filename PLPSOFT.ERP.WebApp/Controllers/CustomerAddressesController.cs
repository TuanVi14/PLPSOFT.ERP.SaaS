using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class CustomerAddressesController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerAddressesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Danh sách địa chỉ của 1 khách hàng
        public async Task<IActionResult> Index(long customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return NotFound();

            var addresses = await _context.CustomerAddresses
                .Where(a => a.CustomerId == customerId && !a.IsDeleted)
                .ToListAsync();

            ViewBag.CustomerId = customerId;
            ViewBag.CustomerName = customer.CustomerName;
            return View(addresses);
        }

        // GET: Thêm địa chỉ
        public async Task<IActionResult> Create(long customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return NotFound();

            ViewBag.CustomerId = customerId;
            ViewBag.CustomerName = customer.CustomerName;
            return View(new CustomerAddress { CustomerId = customerId });
        }

        // POST: Thêm địa chỉ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerAddress model)
        {
            if (!ModelState.IsValid)
            {
                var customer = await _context.Customers.FindAsync(model.CustomerId);
                ViewBag.CustomerId = model.CustomerId;
                ViewBag.CustomerName = customer?.CustomerName;
                return View(model);
            }

            // Nếu đặt làm mặc định, bỏ mặc định các địa chỉ cũ
            if (model.IsDefault)
            {
                var existing = await _context.CustomerAddresses
                    .Where(a => a.CustomerId == model.CustomerId && a.IsDefault)
                    .ToListAsync();
                existing.ForEach(a => a.IsDefault = false);
            }

            model.CreatedAt = DateTime.Now;
            _context.CustomerAddresses.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { customerId = model.CustomerId });
        }

        // GET: Sửa địa chỉ
        public async Task<IActionResult> Edit(long id)
        {
            var address = await _context.CustomerAddresses
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.AddressId == id);

            if (address == null) return NotFound();

            ViewBag.CustomerName = address.Customer?.CustomerName;
            return View(address);
        }

        // POST: Sửa địa chỉ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerAddress model)
        {
            if (!ModelState.IsValid)
            {
                var customer = await _context.Customers.FindAsync(model.CustomerId);
                ViewBag.CustomerName = customer?.CustomerName;
                return View(model);
            }

            var entity = await _context.CustomerAddresses.FindAsync(model.AddressId);
            if (entity == null) return NotFound();

            // Nếu đặt làm mặc định, bỏ mặc định các địa chỉ cũ
            if (model.IsDefault && !entity.IsDefault)
            {
                var existing = await _context.CustomerAddresses
                    .Where(a => a.CustomerId == model.CustomerId && a.IsDefault && a.AddressId != model.AddressId)
                    .ToListAsync();
                existing.ForEach(a => a.IsDefault = false);
            }

            entity.ReceiverName = model.ReceiverName;
            entity.Phone = model.Phone;
            entity.Province = model.Province;
            entity.District = model.District;
            entity.Ward = model.Ward;
            entity.Address = model.Address;
            entity.IsDefault = model.IsDefault;
            entity.IsBillingAddress = model.IsBillingAddress;
            entity.IsShippingAddress = model.IsShippingAddress;
            entity.Note = model.Note;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { customerId = entity.CustomerId });
        }

        // POST: Xóa (soft delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.CustomerAddresses.FindAsync(id);
            if (entity == null) return NotFound();

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { customerId = entity.CustomerId });
        }
    }
}