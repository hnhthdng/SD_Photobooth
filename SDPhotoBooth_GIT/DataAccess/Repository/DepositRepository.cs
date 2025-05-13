using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DepositRepository : Repository<Deposit>, IDepositRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public DepositRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Deposit deposit)
        {
            var objFromDb = _db.Deposit.FirstOrDefault(s => s.Id == deposit.Id);
            if (objFromDb != null)
            {
                objFromDb.Amount = deposit.Amount;
                objFromDb.WalletId = deposit.WalletId;
                objFromDb.Status = deposit.Status;
            }
        }

        public Task UpdateAsync(Deposit deposit)
        {
            var objFromDb = _db.Deposit.FirstOrDefault(s => s.Id == deposit.Id);
            if (objFromDb != null)
            {
                objFromDb.Amount = deposit.Amount;
                objFromDb.WalletId = deposit.WalletId;
                objFromDb.Status = deposit.Status;
            }
            return Task.CompletedTask;
        }
    }
}
