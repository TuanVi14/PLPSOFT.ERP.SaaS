using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public class CustomerAddress
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string CustomerId { get; set; }

    [Required]
    [StringLength(255)]
    public string AddressLine { get; set; }

    public string Ward { get; set; }
    public string District { get; set; }
    public string City { get; set; }

    public bool IsDefault { get; set; }
}