namespace MormorDagnysDel2.ViewModels.Orders;

public class OrderViewModel : OrderBaseViewModel
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Quantity { get; set; }
}
