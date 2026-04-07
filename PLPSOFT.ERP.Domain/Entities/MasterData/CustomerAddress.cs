using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
<<<<<<< HEAD

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class CustomerAddress
    {
        public long AddressId { get; set; }

<<<<<<< HEAD
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
=======
        public long CustomerId { get; set; }

        public string ReceiverName { get; set; } = null!;
>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77

        public string? Phone { get; set; }

<<<<<<< HEAD
    // Navigation
    public Customer? Customer { get; set; }
}
=======
=======

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class CustomerAddress
    {
        public long AddressId { get; set; }

        public long CustomerId { get; set; }

        public string ReceiverName { get; set; } = null!;

        public string? Phone { get; set; }

>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77
=======

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class CustomerAddress
    {
        public long AddressId { get; set; }

        public long CustomerId { get; set; }

        public string ReceiverName { get; set; } = null!;

        public string? Phone { get; set; }

>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77
        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public string Address { get; set; } = null!;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsDefault { get; set; }

        public bool IsBillingAddress { get; set; }

        public bool IsShippingAddress { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77
=======
>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77
=======
>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77
