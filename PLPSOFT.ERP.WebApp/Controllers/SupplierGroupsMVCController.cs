using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Sales.SaaS.V2026.Data;

public class SupplierGroupsMVCController : Controller
{
    private readonly AppDbContext _context;

    public SupplierGroupsMVCController(AppDbContext context)
    {
        _context = context;
    }

    // CREATE
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SupplierGroup model)
    {
        model.CompanyID = 1;
        model.IsActive = true;

        _context.SupplierGroups.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // EDIT
    public async Task<IActionResult> Edit(long id)
    {
        var data = await _context.SupplierGroups.FindAsync(id);
        if (data == null) return NotFound();

        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, SupplierGroup model)
    {
        var data = await _context.SupplierGroups.FindAsync(id);
        if (data == null) return NotFound();

        data.GroupName = model.GroupName;
        data.GroupCode = model.GroupCode;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // DELETE
    public async Task<IActionResult> Delete(long id)
    {
        var data = await _context.SupplierGroups.FindAsync(id);
        if (data == null) return NotFound();

        _context.SupplierGroups.Remove(data);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Index(string search)
    {
        var data = _context.SupplierGroups.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            data = data.Where(x => x.GroupName.Contains(search));
        }

        ViewBag.Search = search;

        return View(await data.ToListAsync());
    }
}