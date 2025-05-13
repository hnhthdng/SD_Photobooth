using BussinessObject.Enums;

namespace BusinessLogic.DTO.OrderDTO
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public long Code { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal Amount { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedById { get; set; }
        public string? SessionCode { get; set; }
        public string? PaymentMethodName { get; set; }
        public string? CouponCode { get; set; }
        public string? LocationName { get; set; }
        public string? BoothName { get; set; }
        public string? TypeSessionName { get; set; }
        public string? CustomerId { get; set; }
        public UserResponseOnOrderDTO? User { get; set; }
    }

    public class UserResponseOnOrderDTO
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

}