using MormorDagnysDel2.ViewModels.Customer;

namespace MormorDagnysDel2.Interfaces;

public interface ICustomerRepository
{
    public Task<IList<CustomersViewModel>> List();
    public Task<CustomerViewModel> Find(int id);
    public Task<bool> Add(CustomerPostViewModel model);
    public Task<bool> Update(int id,CustomerBaseViewModel model);
}