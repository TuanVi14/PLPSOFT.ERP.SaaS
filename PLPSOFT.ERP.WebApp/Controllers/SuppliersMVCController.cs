using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Sales.SaaS.V2026.Data;

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
        return View();
    }

    // POST: Create
    [HttpPost]
    public async Task<IActionResult> Create(Supplier model)
    {

        model.CreatedAt = DateTime.Now;

        model.CompanyID = 1;        
        model.SupplierTypeID = 1;

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

        _context.Suppliers.Remove(data);
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