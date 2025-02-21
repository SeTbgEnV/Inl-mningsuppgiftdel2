using System.ComponentModel.DataAnnotations;

namespace MormorDagnysDel2.Entities;

public class SalesOrder
{
    [Key]
    public int SalesOrderId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> OrderItems { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}
