namespace PLPSOFT.ERP.Domain.Entities.MasterData;

    public class ProductUnit
    {
        public long ProductUnitID { get; set; }
        public long CompanyID { get; set; }
        public string UnitCode { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
