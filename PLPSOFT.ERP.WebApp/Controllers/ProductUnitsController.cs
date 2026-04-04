using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;


public class ProductUnitsController : Controller
{
    private readonly AppDbContext _context;

    public ProductUnitsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _context.ProductUnits.ToListAsync();
        return View(data);
    }

    public IActionResult Create()
    {
        PopulateUnitTypes(); 
        return View();
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Create(ProductUnit model)
    {
        if (ModelState.IsValid)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopulateUnitTypes(model.UnitTypeID); // 🔥 giữ dropdown
        return View(model);
    }

    public async Task<IActionResult> Edit(long id)
    {
        var data = await _context.ProductUnits.FindAsync(id);
        if (data == null) return NotFound();

        PopulateUnitTypes(data.UnitTypeID); // 🔥 QUAN TRỌNG

        return View(data);
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Edit(ProductUnit model)
    {
        if (ModelState.IsValid)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopulateUnitTypes(model.UnitTypeID); 
        return View(model);
    }

    public async Task<IActionResult> Delete(long id)
    {
        var data = await _context.ProductUnits.FindAsync(id);
        if (data != null)
        {
            _context.Remove(data);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    private void PopulateUnitTypes(object selected = null)
    {
        ViewBag.UnitTypeID = new SelectList(
            _context.SystemTypeValues
                .Where(x => x.TypeID == 2 && x.IsActive == true), // 🔥 CHUẨN
            "TypeValueID",
            "ValueName",
            selected
        );
    }
}