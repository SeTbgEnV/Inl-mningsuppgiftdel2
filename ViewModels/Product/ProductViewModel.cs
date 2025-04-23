namespace MormorDagnysDel2.ViewModels.Product;

public class ProductViewModel
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public int Weight { get; set; }
    public int QuantityInpack { get; set; }
    public decimal Price { get; set; }
    public string ImageURL { get; set; }
    public string ItemNumber { get; set; }
    public DateTime ExpDate { get; set; }
}
