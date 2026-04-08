using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;

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
        
        ViewBag.Companies = _context.Companies.ToList();
        ViewBag.SupplietType = _context.SystemTypeValues
            .Where(x => x.TypeId == 2) // 👈 rất quan trọng
            .ToList();
        ViewBag.SupplierGroups = new List<SupplierGroup>();
        return View();
    }

    // POST: Create
    [HttpPost]
    public async Task<IActionResult> Create(Supplier model)
    {
        model.CreatedAt = DateTime.Now;

        // lấy công ty
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.CompanyId == model.CompanyId);

        // đếm số supplier trong công ty
        var count = await _context.Suppliers
            .Where(x => x.CompanyId == model.CompanyId)
            .CountAsync() + 1;

        // sinh mã
        model.SupplierCode = $"NCC-{company.CompanyCode}-{count:D3}";

        
        model.IsActive = true;
        model.IsDeleted = false;

        _context.Suppliers.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public JsonResult GetSupplierGroupsByCompany(long companyId)
    {
        var groups = _context.SupplierGroups
            .Where(x => x.CompanyId == companyId && x.IsActive)
            .Select(x => new {
                x.SupplierGroupId,
                x.GroupName
            }).ToList();

        return Json(groups);
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
        data.SupplierGroupId = model.SupplierGroupId;
        data.Phone = model.Phone;
        data.Email = model.Email;
        data.TaxCode = model.TaxCode;

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
            .Include(x => x.SupplierType)
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