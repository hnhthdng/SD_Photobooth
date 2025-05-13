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
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public TransactionRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
