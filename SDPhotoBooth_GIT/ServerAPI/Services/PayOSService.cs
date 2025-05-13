using Net.payOS;
using Net.payOS.Types;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.TypeSessionDTO;
using BusinessLogic.DTO.UserDTO;
using BusinessLogic.Service;
using ServerAPI.Services.IService;

namespace ServerAPI.Services
{

    public class PayOSService : IPayOSService
    {
        private readonly PayOS _payOS;

        public PayOSService(IConfiguration configuration)
        {
            _payOS = new PayOS(
                configuration["PAYOS_CLIENT_ID"],
                configuration["PAYOS_API_KEY"],
                configuration["PAYOS_CHECKSUM_KEY"]
            );
        }

        public async Task<string> CreatePaymentLink(decimal amount, string paymentMethod, OrderResponseDTO order, TypeSessionResponseDTO typeSession, UserResponseDTO user, string clientUrl, string? connectId)
        {
            var url = $"{clientUrl}/confirm-payment-payos";
            if (!string.IsNullOrEmpty(connectId))
            {
                url += $"?connectId={connectId}";
            }
            var paymentData = new PaymentData(
                orderCode: order.Code,
                amount: (int)Math.Round(amount),
                description: $"TECH X ORDER {order.Code}",
                items: new List<ItemData>
                {
                    new ItemData(
                        name: typeSession.Name,
                        quantity: 1,
                        price: (int)typeSession.Price
                    )
                },
                returnUrl: url,
                cancelUrl: url,
                buyerEmail: user == null ? order.Email : user.Email,
                buyerPhone: user == null ? order.Phone : user.PhoneNumber
            );

            var paymentResponse = await _payOS.createPaymentLink(paymentData);

            if (paymentResponse == null || string.IsNullOrEmpty(paymentResponse.checkoutUrl))
            {
                throw new Exception("Failed to create payment link");
            }
            return paymentResponse.checkoutUrl;
        }

        public bool VerifyPaymentWebhookData(WebhookType request)
        {
            try
            {
                _payOS.verifyPaymentWebhookData(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
