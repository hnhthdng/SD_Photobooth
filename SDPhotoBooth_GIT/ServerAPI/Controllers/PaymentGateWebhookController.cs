using AutoMapper;
using BusinessLogic.DTO.GoogleDTO;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.TransactionDTO;
using BusinessLogic.Service;
using BusinessLogic.Service.IService;
using BussinessObject.Enums;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;
using Newtonsoft.Json;
using ServerAPI.Helpers;
using ServerAPI.Services;
using ServerAPI.Services.IService;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGateWebhookController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IPayOSService payOSService;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;
        private readonly ITransactionService transactionService;
        private readonly IDepositService depositService;
        private readonly IMembershipCardService membershipCardService;

        public PaymentGateWebhookController(IOrderService orderService, IPayOSService payOSService, IMapper mapper, IPaymentService paymentService, ITransactionService transactionService, IDepositService depositService, IMembershipCardService membershipCardService)
        {
            this.orderService = orderService;
            this.payOSService = payOSService;
            this.mapper = mapper;
            this.paymentService = paymentService;
            this.transactionService = transactionService;
            this.depositService = depositService;
            this.membershipCardService = membershipCardService;
        }


        [HttpPost("/payos-confirm-webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> PayOSConfirmWebhook([FromBody] WebhookType request)
        {
            try
            {
                if (request == null || request.data == null)
                {
                    return BadRequest(new { code = "20", desc = "Invalid request data" });
                }

                var orderCode = request.data.orderCode;
                var amount = request.data.amount;
                string code = request.data.code;

                if (!payOSService.VerifyPaymentWebhookData(request))
                {
                    return BadRequest(new { code = "20", desc = "Invalid Signature" });
                }

                var status = request.success ? OrderStatus.Completed : OrderStatus.Failed;
                var order = await orderService.UpdateOrderStatusByCode(new OrderStatusCodeRequestDTO { OrderCode = orderCode, Status = status });
                var payment = await paymentService.UpdatePaymentStatusByOrderId(order.Id, request.success ? PaymentStatus.Success : PaymentStatus.Failed);

                await transactionService.CreateTransaction(new TransactionRequestDTO
                {
                    PaymentId = payment.Id,
                    Amount = amount,
                    Type = TransactionType.Purchase,
                    Description = request.data.description,
                    AccountNumber = request.data.accountNumber,
                    Reference = request.data.reference,
                    TransactionDateTime = request.data.transactionDateTime,
                    Currency = request.data.currency,
                    PaymentLinkId = request.data.paymentLinkId,
                    Code = code,
                    Desc = request.data.desc,
                    CounterAccountBankId = request.data.counterAccountBankId,
                    CounterAccountBankName = request.data.counterAccountBankName,
                    CounterAccountName = request.data.counterAccountName,
                    CounterAccountNumber = request.data.counterAccountNumber,
                    VirtualAccountName = request.data.virtualAccountName,
                    VirtualAccountNumber = request.data.virtualAccountNumber
                });

                if (request.success && order.CustomerId != null)
                {
                    await membershipCardService.UpdatePoint(order.CustomerId, (int)Math.Round(order.Amount / 10), true);
                }

                return Ok(new
                {
                    code = "00",
                    desc = "success",
                    data = order
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { code = "20", desc = "Internal Server Error", data = ex.Message });
            }
        }

        [HttpPost("/google-play-purchase-webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> GooglePlayPurchaseWebhook([FromBody] GoogleBillingRequest request)
        {
            try
            {
                if (request == null || request.PurchaseToken == null)
                {
                    return BadRequest(new { code = "20", desc = "Invalid request data" });
                }
                var orderCode = request.OrderCode;
                var depositId = request.DepositId;
                var purchaseToken = request.PurchaseToken;
                var typeSessionId = request.TypeSessionName;

                var purchaseResponse = await GoogleTokenHelper.VerifyPurchaseAsync(typeSessionId, purchaseToken);

                if (purchaseResponse == null || purchaseResponse.PurchaseState != 0)
                {
                    return BadRequest(new { code = "20", desc = "Invalid purchase" });
                }

                if (orderCode.HasValue)
                {
                    var order = await orderService.UpdateOrderStatusByCode(new OrderStatusCodeRequestDTO { OrderCode = orderCode.Value, Status = OrderStatus.Completed });

                    var payment = await paymentService.UpdatePaymentStatusByOrderId(order.Id, PaymentStatus.Success);
                    await transactionService.CreateTransaction(new TransactionRequestDTO
                    {
                        PaymentId = payment.Id,
                        Amount = payment.Amount,
                        Type = TransactionType.Purchase,
                        Reference = purchaseToken,
                        Description = $"Order {order.Code} via Google Billing",
                    });

                    await membershipCardService.UpdatePoint(
                        order.CustomerId,
                        (int)Math.Round(order.Amount / 10),
                        true
                    );
                }
                else if (depositId.HasValue)
                {
                    var deposit = await depositService.UpdateDepositStatusById(depositId.Value, DepositStatus.Success);

                    var payment = await paymentService.UpdatePaymentStatusByDepositId(deposit.Id, PaymentStatus.Success);

                    await transactionService.CreateTransaction(new TransactionRequestDTO
                    {
                        PaymentId = payment.Id,
                        Amount = payment.Amount,
                        Type = TransactionType.Purchase,
                        Description = $"Deposit {deposit.Id} via Google Billing",
                    });
                }


                return Ok(new
                {
                    code = "00",
                    desc = "success",
                    //data = purchaseResponse
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { code = "20", desc = "Internal Server Error", data = ex.Message });
            }
        }

    }
}
