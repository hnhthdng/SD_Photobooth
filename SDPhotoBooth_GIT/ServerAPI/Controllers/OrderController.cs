using Azure.Core;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.PaymentDTO;
using BusinessLogic.DTO.TransactionDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Enums;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Services.IService;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ITypeSessionService typeSessionService;
        private readonly IPaymentMethodService paymentMethodService;
        private readonly IUserService userService;
        private readonly ICouponService couponService;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        private readonly IPayOSService payOSService;
        private readonly IPaymentService paymentService;
        private readonly IWalletService walletService;
        private readonly ITransactionService transactionService;
        private readonly IMembershipCardService membershipCardService;

        public OrderController(IOrderService orderService, ITypeSessionService typeSessionService, IPaymentMethodService paymentMethodService, IUserService userService, ICouponService couponService, IConfiguration configuration, IHostEnvironment env, IPayOSService payOSService, IPaymentService paymentService, IWalletService walletService, ITransactionService transactionService, IMembershipCardService membershipCardService)
        {
            this.orderService = orderService;
            this.typeSessionService = typeSessionService;
            this.paymentMethodService = paymentMethodService;
            this.userService = userService;
            this.couponService = couponService;
            _configuration = configuration;
            _env = env;
            this.payOSService = payOSService;
            this.paymentService = paymentService;
            this.walletService = walletService;
            this.transactionService = transactionService;
            this.membershipCardService = membershipCardService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> GetAllOrders([FromQuery] PaginationParams pagination)
        {
            try
            {
                var result = await orderService.GetAllOrders(pagination);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderCreated()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await orderService.GetOrdersForCurrentUser(userId);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("orders-customer/{id}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> GetOrderByCusId([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Order ID is required.");
            }

            var order = await orderService.GetOrdersByCusId(id);
            return order != null ? Ok(order) : NotFound("Order not found.");
        }

        [HttpGet("{id:long}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (!Enum.TryParse(role, out UserType userType))
            {
                return Unauthorized();
            }

            var order = new OrderResponseDTO();

            switch (userType)
            {
                case UserType.Manager:
                case UserType.Staff:
                    order = await orderService.GetOrder(id, null);
                    break;
                case UserType.Customer:
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    order = await orderService.GetOrder(id, userId);
                    return Ok(order);
                default:
                    return Unauthorized();
            }

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost("dashboard")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> CreateOrderDashboard([FromBody] OrderRequestDTO order)
        {
            try
            {
                return await ProcessOrder(order, isDashboard: true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("desktop")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrderDesktop([FromBody] OrderRequestDTO order)
        {
            try
            {
                return await ProcessOrder(order, isDashboard: false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrderMobile([FromBody] OrderRequestDTO order)
        {
            try
            {
                ModelState.Remove("ConnectionId");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return Unauthorized("User not authenticated.");

                order.CustomerId = userId;

                var typeSession = await typeSessionService.GetTypeSessionCodeByIdAsync(order.TypeSessionId);
                if (typeSession == null) return BadRequest("Invalid type session.");

                var newOrder = await orderService.CreateOrder(order);

                var paymentMethod = await paymentMethodService.GetPaymentMethod(order.PaymentMethodId);
                if (paymentMethod == null || !paymentMethod.IsOnline || !paymentMethod.ForMobile || !paymentMethod.IsActive)
                    return BadRequest("Invalid payment.");

                if (paymentMethod.MethodName == "Wallet")
                {
                    if (order.Amount == null) return BadRequest("Amount is null.");

                    var walletUpdate = await walletService.UpdateBalanceWallet(userId, order.Amount.Value, false);

                    if (walletUpdate != null)
                    {
                        await paymentService.CreatePaymentForOrder(new PaymentRequestDTO
                        {
                            OrderId = newOrder.Id,
                            Amount = newOrder.Amount,
                            PaymentMethodId = order.PaymentMethodId,
                            Status = PaymentStatus.Success
                        });

                        var orderUpdate = await orderService.UpdateOrderStatus(new OrderStatusRequestDTO
                        {
                            OrderId = newOrder.Id,
                            Status = OrderStatus.Completed
                        });

                        await membershipCardService.UpdatePoint(userId, (int)Math.Round(order.Amount.Value / 10), true);

                        return Ok(orderUpdate);
                    }
                }
                else
                {
                    await paymentService.CreatePaymentForOrder(new PaymentRequestDTO
                    {
                        OrderId = newOrder.Id,
                        Amount = newOrder.Amount,
                        PaymentMethodId = order.PaymentMethodId
                    });
                }

                return Ok(newOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<IActionResult> ProcessOrder(OrderRequestDTO order, bool isDashboard)
        {
            ModelState.Remove("ConnectionId");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (order.TypeSessionId <= 0 || order.PaymentMethodId <= 0)
                return BadRequest("Type session or payment method is invalid.");

            var typeSession = await typeSessionService.GetTypeSessionCodeByIdAsync(order.TypeSessionId);
            if (typeSession == null) return BadRequest("Invalid type session.");

            var paymentMethod = await paymentMethodService.GetPaymentMethod(order.PaymentMethodId);

            if (paymentMethod == null || !paymentMethod.IsActive || (isDashboard && paymentMethod.ForMobile) || (!isDashboard && (!paymentMethod.IsOnline || paymentMethod.ForMobile)))
                return BadRequest("Invalid payment.");

            var customer = await userService.UserDetail(order.Email);
            if (customer != null)
                order.CustomerId = customer.Id;

            var (error, _, _) = await couponService.ApplyCouponIfAny(order, typeSession);
            if (error != null) throw new Exception(error);

            if (!paymentMethod.IsOnline)
                order.Status = OrderStatus.Completed;

            var newOrder = await orderService.CreateOrder(order);
            if (newOrder == null) return BadRequest("Failed to create order.");

            if (!paymentMethod.IsOnline)
            {
                var payment = await paymentService.CreatePaymentForOrder(new PaymentRequestDTO
                {
                    OrderId = newOrder.Id,
                    Amount = newOrder.Amount,
                    PaymentMethodId = order.PaymentMethodId,
                    Status = PaymentStatus.Success
                });

                await transactionService.CreateTransaction(new TransactionRequestDTO
                {
                    PaymentId = payment.Id,
                    Amount = payment.Amount,
                    Type = TransactionType.Purchase,
                    Description = $"Order {newOrder.Code} via Cash"
                });

                if (order.CustomerId != null)
                {
                    await membershipCardService.UpdatePoint(order.CustomerId, (int)Math.Round(newOrder.Amount / 10), true);
                }

                return Ok(newOrder);
            }

            if (order.Amount == null) return BadRequest("Amount is null.");

            string clientUrl = GetClientUrl();

            var paymentLink = await payOSService.CreatePaymentLink(order.Amount.Value, paymentMethod.MethodName, newOrder, typeSession, customer, clientUrl, order.ConnectionId);

            await paymentService.CreatePaymentForOrder(new PaymentRequestDTO
            {
                OrderId = newOrder.Id,
                Amount = newOrder.Amount,
                PaymentMethodId = order.PaymentMethodId,
                PaymentLink = paymentLink
            });

            return Ok(new { newOrder, paymentLink });
        }


        private string GetClientUrl()
        {
            var domain = Request.Headers["Origin"].ToString();
            if (domain == _configuration["ClientUrl:Production"] || domain == _configuration["ClientUrl:Development"])
                return domain;
            return _env.IsDevelopment() ? _configuration["ServerUrl:Development"] : _configuration["ServerUrl:Production"];
        }

        [HttpPost("apply-coupon")]
        [Authorize(Roles = "Manager,Staff,Customer")]
        public async Task<IActionResult> ApplyCoupon([FromBody] OrderRequestDTO order)
        {
            var typeSession = await typeSessionService.GetTypeSessionCodeByIdAsync(order.TypeSessionId);
            if (typeSession == null)
                return BadRequest("Invalid session.");

            var (error, updatedOrder, validCoupon) = await couponService.ApplyCouponIfAny(order, typeSession);
            if (error != null)
                throw new Exception(error);

            return Ok(new
            {
                Order = updatedOrder,
                Coupon = validCoupon
            });
        }


        [HttpPatch]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> Patch([FromBody] OrderStatusRequestDTO orderStatus)
        {
            var order = await orderService.GetOrder(orderStatus.OrderId, null);
            if (order == null)
            {
                return BadRequest();
            }

            await orderService.UpdateOrderStatus(orderStatus);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await orderService.GetOrder(id, null);
            if (order == null)
            {
                return NotFound();
            }
            await orderService.DeleteOrder(id);
            return Ok();
        }

        [HttpGet("count")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> GetCount()
        {
            var count = await orderService.GetCount();
            return Ok(count);
        }
    }
}