using Microsoft.EntityFrameworkCore;
using MormorDagnysDel2.Data;
using MormorDagnysDel2.Entities;
using MormorDagnysDel2.Helpers;
using MormorDagnysDel2.Interfaces;
using MormorDagnysDel2.ViewModels.Orders;
using MormorDagnysDel2.ViewModels.Product;

namespace MormorDagnysDel2.Repositories;

public class ProductRepository(IAddressRepository repo, DataContext context) : IProductRepository
{
    private readonly DataContext _context = context;
    private readonly IAddressRepository _repo = repo;

    public async Task<IList<ProductViewModel>> List()
    {
        try
        {
            var products = await _context.Products.ToListAsync();

            IList<ProductViewModel> response = new List<ProductViewModel>();

            foreach (var product in products)
            {
                var view = new ProductViewModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,
                    ImageURL = product.ImageURL,
                    Weight = product.Weight,
                    ItemNumber = product.ItemNumber,
                    QuantityInpack = product.QuantityInpack,
                };

                response.Add(view);
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new DagnysException($"Ett Fel uppstod med lista allt): {ex.Message}");
        }
    }
    public async Task<Product> Get(int id)
    {
        var product = await _context.Products
            .Where(product => product.Id == id)
            .SingleOrDefaultAsync();
        if (product == null)
        {
            throw new DagnysException("Produkt hittades inte");
        }
        return product;
    }
    public async Task<Product> Add(ProductPostViewModel product)
    {
        var existingProduct = await _context.Products
            .Where(p => p.ItemNumber == product.ItemNumber)
            .SingleOrDefaultAsync();
        if (existingProduct != null)
        {
            throw new DagnysException("Produkten existerar redan");
        }
        var newProduct = new Product
        {
            ProductName = product.ProductName,
            Description = product.Description,
            Price = product.Price,
            Weight = product.Weight,
            ImageURL = product.ImageURL,
            ItemNumber = product.ItemNumber,
            QuantityInpack = product.QuantityInpack,
        };

        _context.Products.Add(newProduct);
        return newProduct;
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new DagnysException("Produkt hittades inte");
            }
            _context.Products.Remove(product);
            return true;
        }
        catch (Exception ex)
        {
            throw new DagnysException($"Fel uppstod, försök igen): {ex.Message}");
        }
    }

    public async Task<Product> UpdateProductPrice(int id, ProductPriceViewModel product)
    {
        var productToUpdate = await _context.Products.FindAsync(id);

        if (productToUpdate == null)
        {
            throw new DagnysException("Product hittades inte");
        }

        productToUpdate.Price = product.Price;

        await _context.SaveChangesAsync();
        return productToUpdate;
    }

    public async Task<IList<OrderViewModel>> OrdersByProduct(int id)
    {
        var productExists = await _context.Products.AnyAsync(p => p.Id == id);
        if (!productExists)
        {
            throw new DagnysException($"Produkten med id {id} existerar inte");
        }

        var orders = await _context.OrderItems
            .Where(o => o.ProductId == id)
            .Include(o => o.SalesOrder)
            .ThenInclude(o => o.Customer)
            .Include(o => o.Product)
            .Include(o => o.SalesOrder.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        if (orders == null)
        {
            throw new DagnysException($"Inga beställningar hittades för produkten med id {id}");
        }

        IList<OrderViewModel> response = new List<OrderViewModel>();

        foreach (var orderItem in orders)
        {
            var view = new OrderViewModel
            {
                Id = orderItem.SalesOrder.SalesOrderId,
                Quantity = orderItem.Quantity,
                OrderDate = orderItem.SalesOrder.OrderDate,
                TotalPrice = orderItem.SalesOrder.OrderItems.Sum(o => o.Quantity * o.Product.Price),
                CustomerId = orderItem.SalesOrder.CustomerId,
                FirstName = orderItem.SalesOrder.Customer.FirstName,
                LastName = orderItem.SalesOrder.Customer.LastName,
                Email = orderItem.SalesOrder.Customer.Email,
                OrderItems = orderItem.SalesOrder.OrderItems.Select(o => new OrderItemViewModel
                {
                    Id = o.ProductId,
                    Product = o.Product.ProductName,
                    Quantity = o.Quantity,
                    Price = (double)o.Product.Price
                }).ToList()
            };

            response.Add(view);
        }

        return response;
    }
}