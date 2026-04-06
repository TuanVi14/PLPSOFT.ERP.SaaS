namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public class CustomerGroup
{
    // Giữ nguyên các ID cũ của bạn
    public long CustomerGroupID { get; set; }
    public long CompanyID { get; set; }

    public string GroupCode { get; set; }
    public string GroupName { get; set; }

    public bool IsActive { get; set; }

    // THÊM DÒNG NÀY: Để logic Xóa mềm (Soft Delete) hoạt động
    public bool IsDelete { get; set; } = false;

    // Navigation
    public List<Customer> Customers { get; set; }
}