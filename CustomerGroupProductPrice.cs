public class CustomerGroupProductPrice
{
    [Key] // Thêm dòng này
    public int Id { get; set; }

    // Các thu?c tính khác...
    public int CustomerGroupId { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
}
