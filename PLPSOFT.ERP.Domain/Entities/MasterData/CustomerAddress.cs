using System;
using System.ComponentModel.DataAnnotations;

namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public class CustomerAddress
{
    [Key]
    public long CustomerAddressID { get; set; }

    // Alias để khớp với code Controller gọi .Id
    public long Id => CustomerAddressID;

    public long CustomerID { get; set; }

    // Alias để khớp với code Controller gọi .CustomerId
    public long CustomerId { get => CustomerID; set => CustomerID = value; }

    public long CompanyID { get; set; }

    public string AddressLine1 { get; set; } = string.Empty;

    // Alias để khớp với code Controller gọi .AddressLine
    public string AddressLine { get => AddressLine1; set => AddressLine1 = value; }

    public string? AddressLine2 { get; set; }
    public string? City { get; set; }

    // Giả lập Ward và District để không lỗi Build (vì DB của ông dùng City/State)
    public string? Ward { get => City; set => City = value; }
    public string? District { get => State; set => State = value; }

    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public bool IsDelete { get; set; } = false; // Thêm để xóa mềm

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public virtual Customer? Customer { get; set; }
}