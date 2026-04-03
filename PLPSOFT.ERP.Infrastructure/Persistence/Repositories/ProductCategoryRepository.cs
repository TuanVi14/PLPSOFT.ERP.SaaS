using Dapper;
using Microsoft.Data.SqlClient;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Repositories;
    public class ProductCategoryRepository
    {
        private readonly string _connectionString;

        public ProductCategoryRepository(IConfiguration configuration)
        {
            // Lấy chuỗi kết nối từ file appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Hàm lấy danh sách Category theo ID công ty (Đúng chuẩn SaaS trong ERD)
        public async Task<IEnumerable<ProductCategory>> GetAllByCompanyAsync(long companyId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // Truy vấn lấy các danh mục đang hoạt động của công ty cụ thể
                string sql = "SELECT * FROM ProductCategories WHERE CompanyID = @CompID AND IsActive = 1";
                return await db.QueryAsync<ProductCategory>(sql, new { CompID = companyId });
            }
        }
    }
