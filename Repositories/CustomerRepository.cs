using Microsoft.EntityFrameworkCore;
using MormorDagnysDel2.Data;
using MormorDagnysDel2.Entities;
using MormorDagnysDel2.Helpers;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.ViewModels.Address;
using MormorDagnysDel2.ViewModels.Customer;
using MormorDagnysDel2.ViewModels.Orders;

namespace MormorDagnysDel2.Repositories;

public class CustomerRepository(DataContext context, IAddressRepository repo) : ICustomerRepository
{
    private readonly DataContext _context = context;
    private readonly IAddressRepository _repo = repo;

    public async Task<bool> Add(CustomerPostViewModel model)
    {
        try
        {
            if (await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower().Trim()
              == model.Email.ToLower().Trim()) != null)
            {
                throw new DagnysException("Kunden finns redan");
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone
            };

            await _context.AddAsync(customer);

            foreach (var a in model.Addresses)
            {
                var address = await _repo.Add(a);

                await _context.CustomerAddresses.AddAsync(new CustomerAddress
                {
                    Address = address,
                    Customer = customer
                });
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<CustomerViewModel> Find(int id)
    {

        try
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Include(c => c.CustomerAddresses)
                    .ThenInclude(c => c.Address)
                    .ThenInclude(c => c.PostalAddress)
                .Include(c => c.CustomerAddresses)
                    .ThenInclude(c => c.Address)
                    .ThenInclude(c => c.AddressType)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .SingleOrDefaultAsync();

            if (customer is null)
            {
                throw new DagnysException($"Det finns ingen kund med id {id}");
            }

            var view = new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Addresses = customer.CustomerAddresses.Select(c => new AddressViewModel
                {
                    AddressLine = c.Address.AddressLine,
                    PostalCode = c.Address.PostalAddress.PostalCode,
                    City = c.Address.PostalAddress.City,
                    AddressType = c.Address.AddressType.Value
                }).ToList(),
                Orders = customer.Orders.Select(o => new OrderBaseViewModel
                {
                    Id = o.CustomerId,
                    OrderDate = o.OrderDate,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        Id = oi.Product.Id,
                        Product = oi.Product.ProductName,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                }).ToList()
            };

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<CustomersViewModel>> List()
    {
        var response = await _context.Customers.ToListAsync();
        var customers = response.Select(c => new CustomersViewModel
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Phone = c.Phone
        });

        return [.. customers];
    }

    public async Task<bool> Update(int id,CustomerBaseViewModel model)
    {
        var customerupdate = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (customerupdate == null)
        {
            throw new DagnysException($"Det finns ingen kund med id {id}");
        }
        customerupdate.FirstName = model.FirstName;
        customerupdate.LastName = model.LastName;
        customerupdate.Email = model.Email;
        customerupdate.Phone = model.Phone;

        return true;
    }
}