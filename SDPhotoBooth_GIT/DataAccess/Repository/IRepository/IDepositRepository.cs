using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IDepositRepository : IRepository<Deposit>
    {
        void Update(Deposit deposit);
        Task UpdateAsync(Deposit deposit);
    }
}
