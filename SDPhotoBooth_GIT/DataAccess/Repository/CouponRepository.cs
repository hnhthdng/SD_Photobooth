using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly AIPhotoboothDbContext _context;
        public CouponRepository(AIPhotoboothDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Coupon coupon)
        {
            var objFromDb = _context.Coupon.FirstOrDefault(s => s.Id == coupon.Id);
            objFromDb.Name = coupon.Name;
            objFromDb.Discount = coupon.Discount;
            objFromDb.DiscountPercent = coupon.DiscountPercent;
            objFromDb.Description = coupon.Description;
            objFromDb.Code = coupon.Code;
            objFromDb.StartDate = coupon.StartDate;
            objFromDb.EndDate = coupon.EndDate;
            objFromDb.MaxUse = coupon.MaxUse;
            objFromDb.UsedAmount = coupon.UsedAmount;
            objFromDb.MaxDiscount = coupon.MaxDiscount;
            objFromDb.MinOrder = coupon.MinOrder;
            objFromDb.IsActive = coupon.IsActive;
        }

        public Task UpdateAsync(Coupon coupon)
        {
            var objFromDb = _context.Coupon.FirstOrDefault(s => s.Id == coupon.Id);
            objFromDb.Name = coupon.Name;
            objFromDb.Discount = coupon.Discount;
            objFromDb.DiscountPercent = coupon.DiscountPercent;
            objFromDb.Description = coupon.Description;
            objFromDb.Code = coupon.Code;
            objFromDb.StartDate = coupon.StartDate;
            objFromDb.EndDate = coupon.EndDate;
            objFromDb.MaxUse = coupon.MaxUse;
            objFromDb.UsedAmount = coupon.UsedAmount;
            objFromDb.MaxDiscount = coupon.MaxDiscount;
            objFromDb.MinOrder = coupon.MinOrder;
            objFromDb.IsActive = coupon.IsActive;
            return Task.CompletedTask;
        }
    }
}
