using MormorDagnysDel2.Entities;
using MormorDagnysDel2.ViewModels.Orders;
using MormorDagnysDel2.ViewModels.Product;

namespace MormorDagnysDel2.Interfaces;

public interface IProductRepository
{
    public Task<IList<ProductViewModel>> List();
    public Task<Product> Get(int id);
    public Task<Product> Add(ProductPostViewModel product);
    public Task<Product> UpdateProductPrice(int id, ProductPriceViewModel product);
    public Task<bool> Delete(int id);
    public Task<IList<OrderViewModel>> OrdersByProduct(int id);
}