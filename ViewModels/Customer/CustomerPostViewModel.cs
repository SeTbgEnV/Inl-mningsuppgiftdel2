using MormorDagnysDel2.ViewModels.Address;

namespace MormorDagnysDel2.ViewModels.Customer;

public class CustomerPostViewModel : CustomerBaseViewModel
{
    public IList<AddressPostViewModel> Addresses { get; set; }
}