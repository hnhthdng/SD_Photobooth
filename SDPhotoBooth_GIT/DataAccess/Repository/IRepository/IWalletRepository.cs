using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        void Update(Wallet wallet);
        Task UpdateAsync(Wallet wallet);    
    }
}
