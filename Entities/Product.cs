namespace MormorDagnysDel2.Entities;

public class Product
{
    public int Id { get; set; }
    public string ItemNumber { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Weight { get; set; }
    public int QuantityInpack { get; set; }
    public string ImageURL { get; set; }
    public string Description { get; set; }
    public IList<OrderItem> OrderItems { get; set; }
}