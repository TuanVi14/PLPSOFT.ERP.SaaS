using Dapper;
using Microsoft.Data.SqlClient;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Repositories;

public class ProductUnitRepository
    {
        private readonly string _connectionString;
        public ProductUnitRepository(IConfiguration config)
            => _connectionString = config.GetConnectionString("DefaultConnection")!;

        public async Task<IEnumerable<dynamic>> GetAllAsync(long companyId)
        {
            using var db = new SqlConnection(_connectionString);
            // Bỏ lọc CompanyID vì bảng của giảng viên không có cột này
            string sql = "SELECT UnitID, UnitName FROM dbo.ProductUnits WHERE IsActive = 1";
            return await db.QueryAsync(sql);
        }
    }