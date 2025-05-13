using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public PaymentMethodRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(PaymentMethod payment)
        {
            var objFromDb = _db.PaymentMethod.FirstOrDefault(s => s.Id == payment.Id);
            objFromDb.MethodName = payment.MethodName;
            objFromDb.Description = payment.Description;
            objFromDb.IsActive = payment.IsActive;
        }

        public Task UpdateAsync(PaymentMethod payment)
        {
            var objFromDb = _db.PaymentMethod.FirstOrDefault(s => s.Id == payment.Id);
            objFromDb.MethodName = payment.MethodName;
            objFromDb.Description = payment.Description;
            objFromDb.IsActive = payment.IsActive;
            return Task.CompletedTask;
        }
    }
}
