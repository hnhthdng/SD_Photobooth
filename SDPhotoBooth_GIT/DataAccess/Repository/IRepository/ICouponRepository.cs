using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        void Update(Coupon coupon);
        Task UpdateAsync(Coupon coupon);
    }
}
