using MormorDagnysDel2.Entities;
using MormorDagnysDel2.ViewModels.Orders;

namespace MormorDagnysDel2.Interfaces;

public interface IOrderRepository
{
    public Task<List<OrderViewModel>> ListAllOrders();
    public Task<OrderViewModel> FindOrder(int id);
    public Task<SalesOrder> AddOrder(SalesOrderViewModel order);
    public Task<bool> DeleteOrder(int id);
    public Task<List<OrderViewModel>> FindByDate(string date);
}
