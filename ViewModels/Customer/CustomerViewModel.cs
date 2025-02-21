using MormorDagnysDel2.ViewModels.Address;
using MormorDagnysDel2.ViewModels.Orders;

namespace MormorDagnysDel2.ViewModels.Customer;

public class CustomerViewModel : CustomerBaseViewModel
{
    public int Id { get; set; }
    public IList<AddressViewModel> Addresses { get; set; }
    public IList<OrderBaseViewModel> Orders { get; set; }
}
