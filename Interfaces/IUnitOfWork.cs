namespace MormorDagnysDel2.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }
    IAddressRepository AddressRepository { get; }
    IProductRepository ProductRepository { get; }
    IOrderRepository OrderRepository { get; }

    Task<bool> Complete();
    bool HasChanges();
}
