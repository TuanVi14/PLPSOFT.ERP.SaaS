using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.WebApp.Models;


public class ProductUnitsController : Controller
{
    private readonly AppDbContext _context;

    public ProductUnitsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        // Trả về thẳng Entity cho trang danh sách để nhanh gọn
        var query = _context.ProductUnits.Include(u => u.UnitType).AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.Trim();
            query = query.Where(u => u.UnitName.Contains(searchString)
                                  || u.UnitCode.Contains(searchString));
        }
        return View(await query.ToListAsync());
    }

    public IActionResult Create()
    {
        var vm = new ProductUnitViewModel
        {
            // Lấy các giá trị 7, 8, 9 từ bảng SystemTypeValues dựa trên TypeID = 3
            UnitTypeOptions = _context.SystemTypeValues
            .Where(v => v.TypeId == 3)
            .Select(v => new SelectListItem { Value = v.TypeValueId.ToString(), Text = v.ValueName })
            .ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductUnitViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var unit = new ProductUnit
            {
                UnitCode = vm.UnitCode,
                UnitName = vm.UnitName,
                UnitTypeId = vm.UnitTypeID, // Gán giá trị 7, 8 hoặc 9 từ form
                IsActive = vm.IsActive
            };
            _context.ProductUnits.Add(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Nếu lỗi, nạp lại danh sách dropdown
        vm.UnitTypeOptions = _context.SystemTypeValues.Where(v => v.TypeId == 3)
            .Select(v => new SelectListItem { Value = v.TypeValueId.ToString(), Text = v.ValueName }).ToList();
        return View(vm);
    }

    public async Task<IActionResult> Edit(long id)
    {
        var unit = await _context.ProductUnits.FindAsync(id);
        if (unit == null) return NotFound();

        var vm = new ProductUnitViewModel
        {
            UnitID = unit.UnitId,
            UnitCode = unit.UnitCode,
            UnitName = unit.UnitName,
            UnitTypeID = unit.UnitTypeId,
            IsActive = unit.IsActive,
            UnitTypeOptions = _context.SystemTypeValues.Where(v => v.TypeId == 3)
                .Select(v => new SelectListItem { Value = v.TypeValueId.ToString(), Text = v.ValueName }).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductUnitViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var unit = await _context.ProductUnits.FindAsync(vm.UnitID);
            if (unit == null) return NotFound();

            unit.UnitCode = vm.UnitCode;
            unit.UnitName = vm.UnitName;
            unit.UnitTypeId = vm.UnitTypeID;
            unit.IsActive = vm.IsActive;

            _context.Update(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    // POST: ProductUnits/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        var unit = await _context.ProductUnits.FindAsync(id);
        if (unit == null) return Json(new { success = false, message = "Không tìm thấy đơn vị tính!" });

        // Kiểm tra ràng buộc (Ví dụ: ĐVT đã có sản phẩm sử dụng thì tùy bạn có cho ngừng hay không)
        unit.IsActive = false;

        _context.Update(unit);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Đã chuyển đơn vị tính sang trạng thái 'Ngừng hoạt động'." });
    }

    // POST: ProductUnits/Restore/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(long id)
    {
        var unit = await _context.ProductUnits.FindAsync(id);
        if (unit == null) return Json(new { success = false, message = "Không tìm thấy đơn vị tính!" });

        unit.IsActive = true;

        _context.Update(unit);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Đã phục hồi đơn vị tính thành công." });
    }
}