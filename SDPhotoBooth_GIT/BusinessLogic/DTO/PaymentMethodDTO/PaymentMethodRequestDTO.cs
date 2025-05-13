namespace BusinessLogic.DTO.PaymentMethod
{
    public class PaymentMethodRequestDTO
    {
        public string? MethodName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }

        public bool? ForMobile { get; set; } = false;
        public bool? IsOnline { get; set; } = false;
    }
}
