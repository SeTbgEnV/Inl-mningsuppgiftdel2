using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MormorDagnysDel2.Data;
using MormorDagnysDel2.Entities;
using MormorDagnysDel2.Helpers;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.ViewModels.Orders;

namespace MormorDagnysDel2.Repositories;

public class OrderRepository(IAddressRepository repo, DataContext context) : IOrderRepository
{
    private readonly DataContext _context = context;
    private readonly IAddressRepository _repo = repo;
    public async Task<SalesOrder> AddOrder(SalesOrderViewModel order)
    {
        if (order.Products == null || !order.Products.Any())
        {
            throw new DagnysException("Order måste innehålla minst en produkt");
        }
        var customer = await _context.Customers
        .SingleOrDefaultAsync(c => c.Id == order.CustomerId);

        if (customer == null)
        {
            throw new Exception("Kunden existerar inte");
        }
        var address = await _context.CustomerAddresses
        .Include(a => a.Customer)
        .ThenInclude(c => c.CustomerAddresses)
        .Include(a => a.Address)
        .ThenInclude(a => a.PostalAddress)
        .Include(a => a.Address)
        .ThenInclude(a => a.AddressType)
        .SingleOrDefaultAsync(a => a.CustomerId == order.CustomerId);
        if (address == null)
        {
            throw new Exception("Adressen existerar inte");
        }

        var newOrder = new SalesOrder
        {
            CustomerId = order.CustomerId,
            OrderDate = DateTime.Today,
            OrderItems = []
        };
        foreach (var product in order.Products)
        {
            var prod = await _context.Products
            .SingleOrDefaultAsync(p => p.Id == product.ProductId);

            if (prod is null) throw new DagnysException($"Produkt Id existerar inte");

            var item = new OrderItem
            {
                Price = (double)product.Price,
                Quantity = product.Quantity,
                ProductId = product.ProductId
            };
            newOrder.OrderItems.Add(item);
        }
        await _context.SalesOrders.AddAsync(newOrder);
        DateTime now = DateTime.Now;
        string Dateoforder = now.ToString("yyyy-MM-dd");

        return newOrder;
    }

    public async Task<bool> DeleteOrder(int id)
    {
        var toDelete = await _context.SalesOrders.FindAsync(id);
        _context.SalesOrders.Remove(toDelete);
        return true;
    }

    public async Task<List<OrderViewModel>> FindByDate(string date)
    {
        var orders = await _context.SalesOrders
        .Include(c => c.OrderItems)
        .ThenInclude(oi => oi.Product)
        .Include(o => o.Customer)
        .Where(o => o.OrderDate == DateTime.Parse(date))
        .OrderByDescending(o => o.OrderDate)
        .Select(order => new OrderViewModel
        {
            Id = order.SalesOrderId,
            OrderDate = order.OrderDate,
            OrderItems = order.OrderItems
              .Select(item => new OrderItemViewModel
              {
                  Id = item.ProductId,
                  Product = item.Product.ProductName,
                  Quantity = item.Quantity,
                  Price = item.Price
              })
              .ToList(),

            TotalPrice = order.OrderItems.Sum(item => (decimal)item.Price * item.Quantity),
            CustomerId = order.CustomerId,
            FirstName = order.Customer.FirstName,
            LastName = order.Customer.LastName,
            Email = order.Customer.Email
        })
          .ToListAsync();
        DateTime now = DateTime.Now;
        string Dateoforder = now.ToString("yyyy-MM-dd-HH");
        return orders;
    }

    public async Task<OrderViewModel> FindOrder(int id)
    {
        var order = await _context.SalesOrders
        .Where(o => o.SalesOrderId == id)
        .Include(c => c.OrderItems)
        .Include(o => o.Customer)
        .Select(order => new OrderViewModel
        {
            Id = order.SalesOrderId,
            OrderDate = order.OrderDate,
            OrderItems = order.OrderItems
              .Select(item => new OrderItemViewModel
              {
                  Id = item.ProductId,
                  Product = item.Product.ProductName,
                  Quantity = item.Quantity,
                  Price = item.Price
              })
              .ToList(),

            TotalPrice = order.OrderItems.Sum(item => (decimal)item.Price * item.Quantity),
            CustomerId = order.CustomerId,
            FirstName = order.Customer.FirstName,
            LastName = order.Customer.LastName,
            Email = order.Customer.Email,
        })
          .SingleOrDefaultAsync();

        return order;
    }

    public async Task<List<OrderViewModel>> ListAllOrders()
    {
        var orders = await _context.SalesOrders
        .Include(c => c.OrderItems)
        .Include(o => o.Customer)
        .OrderByDescending(o => o.OrderDate)
        .Select(order => new OrderViewModel
        {
            Id = order.SalesOrderId,
            OrderDate = order.OrderDate,
            OrderItems = order.OrderItems
              .Select(item => new OrderItemViewModel
              {
                  Id = item.ProductId,
                  Product = item.Product.ProductName,
                  Quantity = item.Quantity,
                  Price = item.Price
              })
              .ToList(),

            TotalPrice = order.OrderItems.Sum(item => (decimal)item.Price * item.Quantity),
            FirstName = order.Customer.FirstName,
            LastName = order.Customer.LastName,
            Email = order.Customer.Email
        })
          .ToListAsync();

        return orders;
    }

}
