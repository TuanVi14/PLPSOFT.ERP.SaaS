USE [PLPSOFT_ERP_SALES_SAAS_V2026];
GO

-- Tắt tất cả kiểm tra khóa ngoại để không bị báo lỗi thiếu bảng gốc
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

--  Nạp dữ liệu cho Nam (Suppliers)
INSERT INTO [dbo].[Suppliers] 
    ([CompanyID], [SupplierCode], [SupplierName], [Phone], [Email], [SupplierTypeID], [SupplierGroupID])
VALUES 
    (1, 'NCC001', N'Công ty TNHH Nam Phát', '0912345678', 'nam@erp.com', 1, 1);

-- Nạp dữ liệu cho Phát (Products)
INSERT INTO [dbo].[Products] 
    ([CompanyID], [ProductCode], [ProductName], [CostPrice], [StandardPrice], [IsActive], [IsDeleted], [CreatedAt], [ProductTypeID], [BaseUnitID], [CategoryID])
VALUES 
    (1, 'SP001', N'Laptop Gaming Asus ROG', 20000000, 25000000, 1, 0, GETDATE(), 1, 1, 1),
    (1, 'SP002', N'Chuột Logitech G502', 800000, 1200000, 1, 0, GETDATE(), 1, 1, 1);

-- Bật lại kiểm tra khóa ngoại sau khi nạp xong
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';

PRINT 'XONG! DU LIEU DA VAO. MO SWAGGER TEST THOI!';

-- Kiểm tra kết quả ngay tại đây
SELECT * FROM Suppliers;
SELECT * FROM Products;