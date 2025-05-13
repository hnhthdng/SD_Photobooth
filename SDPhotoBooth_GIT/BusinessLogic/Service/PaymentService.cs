using AutoMapper;
using BusinessLogic.DTO.PaymentDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Enums;
using BussinessObject.Models;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.Payment.CountAsync();
        }
        public async Task<PaymentResponseDTO> CreatePaymentForOrder(PaymentRequestDTO paymentRequest)
        {
            var payment = _mapper.Map<Payment>(paymentRequest);

            await _unitOfWork.Payment.AddAsync(payment);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<PaymentResponseDTO>(payment);
        }

        public async Task<PaymentResponseDTO> CreatePaymentForDeposit(PaymentRequestDTO paymentRequest)
        {
            var payment = _mapper.Map<Payment>(paymentRequest);

            await _unitOfWork.Payment.AddAsync(payment);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<PaymentResponseDTO>(payment);
        }

        public async Task<PaymentResponseDTO> UpdatePaymentStatusByOrderId(int orderId, PaymentStatus paymentStatus)
        {
            var payment = await _unitOfWork.Payment.GetFirstOrDefaultAsync(t => t.OrderId == orderId);
            if (payment == null)
            {
                return null;
            }
            payment.Status = paymentStatus;
            _unitOfWork.Payment.Update(payment);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PaymentResponseDTO>(payment);
        }

        public async Task<PaymentResponseDTO> UpdatePaymentStatusByDepositId(int depositId, PaymentStatus paymentStatus)
        {
            var payment = await _unitOfWork.Payment.GetFirstOrDefaultAsync(t => t.DepositId == depositId);
            if (payment == null)
            {
                return null;
            }
            payment.Status = paymentStatus;
            _unitOfWork.Payment.Update(payment);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PaymentResponseDTO>(payment);
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetAllPayments(PaginationParams? pagination)
        {
            var payments = await _unitOfWork.Payment.GetAllAsync(
                includeProperties: "Order,PaymentMethod",
                orderBy: o => o.OrderByDescending(o => o.CreatedAt),
                pagination: pagination
            );

            var sorted = payments.ToList();

            return _mapper.Map<IEnumerable<PaymentResponseDTO>>(sorted);
        }

        public Task<PaymentResponseDTO> GetPaymentById(int id)
        {
            var payment = _unitOfWork.Payment.GetFirstOrDefaultAsync(t => t.Id == id, includeProperties: "Order,PaymentMethod");
            if (payment == null)
            {
                return null;
            }
            var paymentDTO = _mapper.Map<PaymentResponseDTO>(payment);
            return Task.FromResult(paymentDTO);
        }

        public Task<PaymentResponseDTO> GetPaymentByCusId(string cusId)
        {
            var payment = _unitOfWork.Payment.GetFirstOrDefaultAsync(t => t.Order.CustomerId == cusId, includeProperties: "Order,PaymentMethod");
            if (payment == null)
            {
                return null;
            }
            var paymentDTO = _mapper.Map<PaymentResponseDTO>(payment);
            return Task.FromResult(paymentDTO);
        }
    }
}
