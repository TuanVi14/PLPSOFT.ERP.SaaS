using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Sales.SaaS.V2026.Data;
using PLPSOFT.ERP.Sales.SaaS.V2026.Models;

public class SuppliersMVCController : Controller
{
    private readonly AppDbContext _context;

    public SuppliersMVCController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Create
    public IActionResult Create()
    {
        ViewBag.SupplierGroups = _context.SupplierGroups.ToList();
        ViewBag.Companies = _context.Companies.ToList();
        return View();
    }

    // POST: Create
    [HttpPost]
    public async Task<IActionResult> Create(Supplier model)
    {
        model.CreatedAt = DateTime.Now;

        // lấy công ty
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.CompanyID == model.CompanyID);

        // đếm số supplier trong công ty
        var count = await _context.Suppliers
            .Where(x => x.CompanyID == model.CompanyID)
            .CountAsync() + 1;

        // sinh mã
        model.SupplierCode = $"NCC-{company.CompanyCode}-{count:D3}";

        model.SupplierTypeID = 4;
        model.IsActive = true;
        model.IsDeleted = false;

        _context.Suppliers.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // GET
    public async Task<IActionResult> Edit(long id)
    {
        var data = await _context.Suppliers.FindAsync(id);
        if (data == null) return NotFound();
        ViewBag.SupplierGroups = _context.SupplierGroups.ToList();
        return View(data);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Edit(long id, Supplier model)
    {

        var data = await _context.Suppliers.FindAsync(id);
        if (data == null) return NotFound();

        data.SupplierName = model.SupplierName;
        data.SupplierGroupID = model.SupplierGroupID;
        data.Phone = model.Phone;
        data.Email = model.Email;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(long id)
    {
        var data = await _context.Suppliers.FindAsync(id);
        if (data == null) return NotFound();

        data.IsDeleted = true;
        data.IsActive = false;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Restore(long id)
    {
        var data = await _context.Suppliers.FindAsync(id);
        if (data == null) return NotFound();

        data.IsDeleted = false;
        data.IsActive = true;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index(string search)
    {
        var data = _context.Suppliers
            .Include(x => x.SupplierGroup)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            data = data.Where(x =>
                x.SupplierCode.Contains(search) ||
                x.SupplierName.Contains(search));
        }

        ViewBag.Search = search;

        return View(await data.ToListAsync());
    }
}