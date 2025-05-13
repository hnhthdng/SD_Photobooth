namespace BusinessLogic.DTO.PaymentMethod
{
    public class PaymentMethodResponseDTO
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string CreatedById { get; set; }
        public string LastModifiedById { get; set; }
        public bool ForMobile { get; set; }
        public bool IsOnline { get; set; }
    }
    
}
