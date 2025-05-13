using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>
    {
        void Update(PaymentMethod paymentMethod);
        Task UpdateAsync(PaymentMethod paymentMethod);
    }
}
