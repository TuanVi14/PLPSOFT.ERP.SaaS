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

        public async Task<IActionResult> Index(string customerId)
        {
            if (string.IsNullOrEmpty(customerId)) return NotFound();

            long parsedCustomerId = long.Parse(customerId);
            ViewBag.CustomerId = customerId;

            var addresses = await _context.CustomerAddresses
                .Where(a => a.CustomerId == parsedCustomerId)
                .ToListAsync();

            return View(addresses);
        }

        // Returns a partial list of addresses for embedding in other views (AJAX)
        public async Task<IActionResult> List(long customerId)
        {
            var addresses = await _context.CustomerAddresses
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();

            return PartialView("_List", addresses);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var customerAddress = await _context.CustomerAddresses
                .FirstOrDefaultAsync(m => m.AddressId == id);

            if (customerAddress == null) return NotFound();

            return View(customerAddress);
        }

        public IActionResult Create(string customerId)
        {
            return View(new CustomerAddress { CustomerId = long.Parse(customerId) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,CustomerId,ReceiverName,Phone,Province,District,Ward,Address,IsDefault,IsBillingAddress,IsShippingAddress,Note")] CustomerAddress customerAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { customerId = customerAddress.CustomerId.ToString() });
            }
            return View(customerAddress);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            if (customerAddress == null) return NotFound();

            return View(customerAddress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AddressId,CustomerId,ReceiverName,Phone,Province,District,Ward,Address,IsDefault,IsBillingAddress,IsShippingAddress,Note")] CustomerAddress customerAddress)
        {
            if (id != customerAddress.AddressId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAddressExists(customerAddress.AddressId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index), new { customerId = customerAddress.CustomerId.ToString() });
            }
            return View(customerAddress);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var customerAddress = await _context.CustomerAddresses
                .FirstOrDefaultAsync(m => m.AddressId == id);

            if (customerAddress == null) return NotFound();

            return View(customerAddress);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            string customerId = customerAddress?.CustomerId.ToString();

            if (customerAddress != null)
            {
                _context.CustomerAddresses.Remove(customerAddress);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { customerId = customerId });
        }

        private bool CustomerAddressExists(long id)
        {
            return _context.CustomerAddresses.Any(e => e.AddressId == id);
        }
    }
}