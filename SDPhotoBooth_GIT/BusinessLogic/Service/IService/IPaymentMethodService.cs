using BusinessLogic.DTO.PaymentMethod;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IPaymentMethodService
    {
        Task<int> CreatePaymentMethod(PaymentMethodRequestDTO paymentMethodRequestDTO);
        Task<PaymentMethodResponseDTO> UpdatePaymentMethod(int id, PaymentMethodRequestDTO paymentMethodRequestDTO);
        Task<int> DeletePaymentMethod(int id);
        Task<PaymentMethodResponseDTO> GetPaymentMethod(int id);
        Task<IEnumerable<PaymentMethodResponseDTO>> GetPaymentMethod(string paymentMethodName);
        Task<IEnumerable<PaymentMethodResponseDTO>> GetAllPaymentMethod(PaginationParams? pagination);
        Task<IEnumerable<PaymentMethodResponseDTO>> GetAllFor(bool forMobile, bool? isOnline);
        Task<int> GetCount();
    }
}
