namespace MormorDagnysDel2.Entities;

public class OrderItem
{
    public int SalesOrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public DateTime ExpDate { get; set; }
    public DateTime BakeDate { get; set; }
    public SalesOrder SalesOrder { get; set; }
    public Product Product { get; set; }
}