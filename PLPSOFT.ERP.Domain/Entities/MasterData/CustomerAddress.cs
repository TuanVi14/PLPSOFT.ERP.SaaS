namespace PLPSOFT.ERP.Domain.Entities.MasterData;
public class CustomerAddress
{
    public long CustomerAddressID { get; set; }
    public long CustomerID { get; set; }
    public long CompanyID { get; set; }

    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    // Coordinates
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Customer? Customer { get; set; }
}