
using BusinessLogic.DTO.CouponDTO;
using BusinessLogic.DTO.LevelMembershipDTO;
using BusinessLogic.DTO.TypeSessionDTO;
using BussinessObject.Models;
using System.Diagnostics;

namespace BusinessLogic.DTO.TypeSessionProductDTO
{
    public class TypeSessionProductResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
        public string? ProductId { get; set; }
        public int? LevelMembershipId { get; set; }
        public int? TypeSessionId { get; set; }
        public int? CouponId { get; set; }
        public LevelMembershipResponseDTO? LevelMembership { get; set; }

        public CouponResponseDTO? Coupon { get; set; }

        public TypeSessionResponseDTO typeSession { get; set; }

        public decimal? Discount => CountDiscount();

        private decimal? CountDiscount()
        {
            if (typeSession == null || (LevelMembership == null && Coupon == null))
            {
                return null;
            }

            decimal discount = 0;
            if (LevelMembership != null)
            {
                if(typeSession.Price >= LevelMembership.MinOrder)
                {
                    discount = typeSession.Price * LevelMembership.DiscountPercent ?? 0;
                    return Math.Min(discount, LevelMembership.MaxDiscount ?? int.MaxValue);
                }
            }
            else if (Coupon != null)
            {
                if (typeSession.Price >= Coupon.MinOrder)
                {
                    discount = Coupon.DiscountPercent != null ? typeSession.Price * Coupon.DiscountPercent ?? 0 : Coupon.Discount ?? 0;
                    return Math.Min(discount, Coupon.MaxDiscount ?? int.MaxValue);
                }
            }

            return discount;
        }
    }
}
