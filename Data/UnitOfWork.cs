using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.Repositories;

namespace MormorDagnysDel2.Data;

public class UnitOfWork(DataContext context, IAddressRepository repo) : IUnitOfWork
{
    private readonly DataContext _context = context;
    private readonly IAddressRepository _repo = repo;
    public ICustomerRepository CustomerRepository => new CustomerRepository(_context, _repo);


    public IAddressRepository AddressRepository => new AddressRepository(_context);

    public IProductRepository ProductRepository => new ProductRepository(_repo, _context);
    public IOrderRepository OrderRepository => new OrderRepository(_repo, _context);


    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}