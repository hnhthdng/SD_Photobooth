using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.TypeSessionDTO;
using BusinessLogic.DTO.UserDTO;
using Net.payOS.Types;

namespace ServerAPI.Services.IService
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentLink(decimal amount, string paymentMethod, OrderResponseDTO order, TypeSessionResponseDTO typeSession, UserResponseDTO user, string clientUrl, string? connectId);
        bool VerifyPaymentWebhookData(WebhookType request);
    }
}
