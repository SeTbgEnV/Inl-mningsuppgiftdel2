using MormorDagnysDel2.ViewModels.Address;
using MormorDagnysDel2.ViewModels.Product;

namespace MormorDagnysDel2.ViewModels.Orders;

public class SalesOrderViewModel : AddressViewModel
{
    public IList<ProductOrderViewModel> Products { get; set; }
    public int CustomerId { get; set; }
}