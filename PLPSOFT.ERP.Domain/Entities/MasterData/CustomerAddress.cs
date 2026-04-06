using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm thư viện này

namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public class CustomerAddress
{
    [Key]
    public long CustomerAddressID { get; set; }

    [NotMapped] // EF sẽ bỏ qua cái này
    public long Id => CustomerAddressID;

    public long CustomerID { get; set; }

    [NotMapped] // EF sẽ bỏ qua cái này, không còn bị trùng cột nữa
    public long CustomerId { get => CustomerID; set => CustomerID = value; }

    public long CompanyID { get; set; }

    public string AddressLine1 { get; set; } = string.Empty;

    [NotMapped] // EF sẽ bỏ qua cái này
    public string AddressLine { get => AddressLine1; set => AddressLine1 = value; }

    public string? AddressLine2 { get; set; }
    public string? City { get; set; }

    [NotMapped] // EF sẽ bỏ qua cái này
    public string? Ward { get => City; set => City = value; }

    [NotMapped] // EF sẽ bỏ qua cái này
    public string? District { get => State; set => State = value; }

    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public bool IsDelete { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public virtual Customer? Customer { get; set; }
}