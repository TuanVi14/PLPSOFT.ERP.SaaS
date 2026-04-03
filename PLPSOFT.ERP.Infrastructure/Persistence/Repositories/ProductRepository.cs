using Dapper;
using Microsoft.Data.SqlClient;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System.Data;
using Microsoft.Extensions.Configuration;
using PLPSOFT.ERP.Domain.Entities;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Repositories;

public class ProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository(IConfiguration config)
            => _connectionString = config.GetConnectionString("DefaultConnection")!;

        public async Task<IEnumerable<Product>> GetAllAsync(long companyId)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            // Câu lệnh SQL nối bảng để lấy tên Danh mục và Đơn vị tính
            string sql = @"
                            SELECT p.*, c.CategoryName, u.UnitName 
                            FROM dbo.Products p
                            LEFT JOIN dbo.ProductCategories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN dbo.ProductUnits u ON p.BaseUnitID = u.UnitID
                            WHERE p.CompanyID = @CompID AND p.IsDeleted = 0";
            return await db.QueryAsync<Product>(sql, new { CompID = companyId });
        }

    public async Task<int> InsertAsync(Product model)
    {
        using var db = new SqlConnection(_connectionString);

        // Sửa cột CategoryID nhận giá trị từ tham số @ProductCategoryID
        string sql = @"INSERT INTO dbo.Products 
                   (CompanyID, ProductCode, ProductName, CategoryID, BaseUnitID, StandardPrice, ProductTypeID, IsActive, IsDeleted) 
                   VALUES 
                   (@CompanyID, @ProductCode, @ProductName, @ProductCategoryID, @BaseUnitID, @StandardPrice, @ProductTypeID, 1, 0)";

        // Dapper sẽ tự động lấy giá trị từ model.ProductCategoryID để lấp vào @ProductCategoryID
        return await db.ExecuteAsync(sql, model);
    }
    public async Task<int> DeleteAsync(long id)
        {
            using var db = new SqlConnection(_connectionString);
            // Cập nhật IsDeleted thành 1 thay vì xóa hẳn để giữ lịch sử dữ liệu
            string sql = "UPDATE dbo.Products SET IsDeleted = 1 WHERE ProductID = @id";
            return await db.ExecuteAsync(sql, new { id });
        }

        public async Task<int> UpdateAsync(Product model)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = @"
                UPDATE dbo.Products 
                SET ProductCode = @ProductCode, 
                    ProductName = @ProductName, 
                    CategoryID = @ProductCategoryID, 
                    BaseUnitID = @BaseUnitID, 
                    StandardPrice = @StandardPrice
                WHERE ProductID = @ProductID";

            return await db.ExecuteAsync(sql, model);
        }
    public async Task<IEnumerable<Product>> SearchAsync(long companyId, string keyword)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            // Sửa lại tên cột cho đúng với ảnh image_380e59.png
            string sql = @"SELECT p.*, 
                              p.CategoryID AS ProductCategoryID, -- Ánh xạ CategoryID về ProductCategoryID
                              p.BaseUnitID,
                              c.CategoryName, 
                              u.UnitName 
                       FROM Products p
                       LEFT JOIN ProductCategories c ON p.CategoryID = c.CategoryID
                       LEFT JOIN ProductUnits u ON p.BaseUnitID = u.UnitID
                       WHERE p.CompanyID = @companyId 
                       AND (p.ProductCode LIKE @key OR p.ProductName LIKE @key)";

            return await conn.QueryAsync<Product>(sql, new
            {
                companyId = companyId,
                key = $"%{keyword}%"
            });
        }
    }

}
