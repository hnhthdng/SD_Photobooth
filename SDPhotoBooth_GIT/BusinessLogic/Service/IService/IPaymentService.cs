using BusinessLogic.DTO.PaymentDTO;
using BussinessObject.Enums;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseDTO>> GetAllPayments(PaginationParams? pagination);
        Task<PaymentResponseDTO> GetPaymentById(int id);
        Task<PaymentResponseDTO> GetPaymentByCusId(string cusId);
        Task<PaymentResponseDTO> CreatePaymentForOrder(PaymentRequestDTO paymentRequest);
        Task<PaymentResponseDTO> CreatePaymentForDeposit(PaymentRequestDTO paymentRequest);
        Task<PaymentResponseDTO> UpdatePaymentStatusByOrderId(int orderId, PaymentStatus paymentStatus);
        Task<PaymentResponseDTO> UpdatePaymentStatusByDepositId(int depositId, PaymentStatus paymentStatus);
        Task<int> GetCount();
    }
}
