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
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public PaymentRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Payment payment)
        {
            var objFromDb = _db.Payment.FirstOrDefault(s => s.Id == payment.Id);
            if (objFromDb != null)
            {
                objFromDb.OrderId = payment.OrderId;
                objFromDb.DepositId = payment.DepositId;
                objFromDb.PaymentLink = payment.PaymentLink;
                objFromDb.PaymentMethodId = payment.PaymentMethodId;
                objFromDb.Status = payment.Status;
                objFromDb.Amount = payment.Amount;
            }
        }

        public Task UpdateAsync(Payment payment)
        {
            var objFromDb = _db.Payment.FirstOrDefault(s => s.Id == payment.Id);
            if (objFromDb != null)
            {
                objFromDb.OrderId = payment.OrderId;
                objFromDb.DepositId = payment.DepositId;
                objFromDb.PaymentLink = payment.PaymentLink;
                objFromDb.PaymentMethodId = payment.PaymentMethodId;
                objFromDb.Status = payment.Status;
                objFromDb.Amount = payment.Amount;
            }
            return Task.CompletedTask;
        }
    }
}
