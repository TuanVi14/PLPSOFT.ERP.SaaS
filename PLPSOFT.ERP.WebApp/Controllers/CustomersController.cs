using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System;
using System.Linq;
using System.Threading.Tasks;
using PLPSOFT.ERP.Infrastructure.Persistence;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var data = _context.Customers
                .Include(x => x.CustomerGroup)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.CustomerCode.Contains(search) || x.CustomerName.Contains(search));
            }

            ViewBag.Search = search;

            return View(await data.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.CustomerGroups = _context.CustomerGroups.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CreatedAt = DateTime.Now;
            model.CompanyId = 1;

            _context.Customers.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var data = await _context.Customers.FindAsync(id);
            if (data == null) return NotFound();
            ViewBag.CustomerGroups = _context.CustomerGroups.ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, Customer model)
        {
            var data = await _context.Customers.FindAsync(id);
            if (data == null) return NotFound();

            data.CustomerName = model.CustomerName;
            data.CustomerCode = model.CustomerCode;
            data.CustomerGroupId = model.CustomerGroupId;
            data.Phone = model.Phone;
            data.Email = model.Email;
            data.CreditLimit = model.CreditLimit;
            data.PaymentTermDays = model.PaymentTermDays;

            data.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id)
        {
            var data = await _context.Customers
                .Include(x => x.CustomerGroup)
                .FirstOrDefaultAsync(x => x.CustomerId == id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var data = await _context.Customers.FindAsync(id);
            if (data == null) return NotFound();

            _context.Customers.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}