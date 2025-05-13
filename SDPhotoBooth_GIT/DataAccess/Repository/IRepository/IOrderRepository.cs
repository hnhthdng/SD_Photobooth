using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
        Task UpdateAsync(Order order);
    }
}
