using MormorDagnysDel2.Entities;
using MormorDagnysDel2.ViewModels.Address;

namespace MormorDagnysDel2.Interfaces;

public interface IAddressRepository
{
  public Task<Address> Add(AddressPostViewModel model);
  public Task<bool> Add(string type);
}
