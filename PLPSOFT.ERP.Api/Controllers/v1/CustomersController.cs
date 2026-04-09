using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLPSOFT.ERP.Infrastructure.Persistence;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Customers
                .Include(c => c.CustomerGroup)
                .Include(c => c.CustomerAddresses)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var item = await _context.Customers
                .Include(c => c.CustomerGroup)
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] System.Text.Json.JsonElement data)
        {
            long companyId = 0;
            if (data.TryGetProperty("companyId", out var pCompanyId) && pCompanyId.ValueKind == System.Text.Json.JsonValueKind.Number)
                companyId = pCompanyId.GetInt64();

            if (companyId == 0)
            {
                var company = await _context.Companies.FirstOrDefaultAsync();
                companyId = company?.CompanyId ?? 0;
            }

            var customer = new Customer();
            customer.CompanyId = companyId;
            if (data.TryGetProperty("customerCode", out var pCode) && pCode.ValueKind == System.Text.Json.JsonValueKind.String)
                customer.CustomerCode = pCode.GetString()!;
            if (data.TryGetProperty("customerName", out var pName) && pName.ValueKind == System.Text.Json.JsonValueKind.String)
                customer.CustomerName = pName.GetString()!;
            if (data.TryGetProperty("customerTypeId", out var pType) && pType.ValueKind == System.Text.Json.JsonValueKind.Number)
                customer.CustomerTypeId = pType.GetInt64();
            if (data.TryGetProperty("customerGroupId", out var pGroup) && pGroup.ValueKind == System.Text.Json.JsonValueKind.Number)
                customer.CustomerGroupId = pGroup.GetInt64();
            if (data.TryGetProperty("phone", out var pPhone) && pPhone.ValueKind == System.Text.Json.JsonValueKind.String)
                customer.Phone = pPhone.GetString();
            if (data.TryGetProperty("email", out var pEmail) && pEmail.ValueKind == System.Text.Json.JsonValueKind.String)
                customer.Email = pEmail.GetString();

            customer.CreatedAt = DateTime.UtcNow;
            customer.IsActive = data.TryGetProperty("isActive", out var pActive) && pActive.ValueKind == System.Text.Json.JsonValueKind.True;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] System.Text.Json.JsonElement data)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null) return NotFound();

            if (data.TryGetProperty("customerCode", out var pCode) && pCode.ValueKind == System.Text.Json.JsonValueKind.String)
                entity.CustomerCode = pCode.GetString()!;
            if (data.TryGetProperty("customerName", out var pName) && pName.ValueKind == System.Text.Json.JsonValueKind.String)
                entity.CustomerName = pName.GetString()!;
            if (data.TryGetProperty("customerTypeId", out var pType) && pType.ValueKind == System.Text.Json.JsonValueKind.Number)
                entity.CustomerTypeId = pType.GetInt64();
            if (data.TryGetProperty("customerGroupId", out var pGroup) && pGroup.ValueKind == System.Text.Json.JsonValueKind.Number)
                entity.CustomerGroupId = pGroup.GetInt64();
            if (data.TryGetProperty("phone", out var pPhone) && pPhone.ValueKind == System.Text.Json.JsonValueKind.String)
                entity.Phone = pPhone.GetString();
            if (data.TryGetProperty("email", out var pEmail) && pEmail.ValueKind == System.Text.Json.JsonValueKind.String)
                entity.Email = pEmail.GetString();

            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var data = await _context.Customers.FindAsync(id);
            if (data == null) return NotFound();

            _context.Customers.Remove(data);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
