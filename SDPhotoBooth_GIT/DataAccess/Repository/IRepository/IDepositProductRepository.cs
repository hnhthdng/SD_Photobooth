using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IDepositProductRepository : IRepository<DepositProduct>
    {
        void Update(DepositProduct depositProduct);
        Task UpdateAsync(DepositProduct depositProduct);
    }
}
