namespace MormorDagnysDel2.ViewModels.Orders;

public class OrderBaseViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public IList<OrderItemViewModel> OrderItems { get; set; }
}
