namespace MormorDagnysDel2.ViewModels.Product;

public class ProductPostViewModel
{
    public string ProductName { get; set; }
    public string ItemNumber { get; set; }
    public string Description { get; set; } 
    public decimal Price { get; set; }
    public int QuantityInpack { get; set; }
    public int Weight { get; set; }
    public string ImageURL { get; set; }
}