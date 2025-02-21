using MormorDagnysDel2.ViewModels.Orders;

namespace MormorDagnysDel2.ViewModels.Product;

public class ProductOrderViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
