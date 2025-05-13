using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public WalletRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Wallet wallet)
        {
            var objFromDb = _db.Wallet.FirstOrDefault(s => s.CustomerId == wallet.CustomerId);
            if (objFromDb != null)
            {
                objFromDb.Balance = wallet.Balance;
                objFromDb.CustomerId = wallet.CustomerId;
                objFromDb.IsLocked = wallet.IsLocked;
            }
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            var objFromDb = await _db.Wallet.FirstOrDefaultAsync(s => s.CustomerId == wallet.CustomerId);
            if (objFromDb != null)
            {
                objFromDb.Balance = wallet.Balance;
                objFromDb.CustomerId = wallet.CustomerId;
                objFromDb.IsLocked = wallet.IsLocked;
            }
        }
    }
}
