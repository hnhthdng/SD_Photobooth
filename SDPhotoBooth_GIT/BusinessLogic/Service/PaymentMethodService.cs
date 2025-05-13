using AutoMapper;
using BusinessLogic.DTO.PaymentMethod;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            var paymentMethods = await _unitOfWork.PaymentMethod.GetAllAsync();
            return paymentMethods.Count();
        }
        public async Task<int> CreatePaymentMethod(PaymentMethodRequestDTO paymentMethodRequestDTO)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodRequestDTO);
            _unitOfWork.PaymentMethod.Add(paymentMethod);
            await _unitOfWork.SaveAsync();
            return paymentMethod.Id;
        }

        public async Task<int> DeletePaymentMethod(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethod.GetFirstOrDefaultAsync(filter: e => e.Id == id);
            _unitOfWork.PaymentMethod.Remove(paymentMethod);
            await _unitOfWork.SaveAsync();
            return id;
        }

        public async Task<IEnumerable<PaymentMethodResponseDTO>> GetAllFor(bool forMobile, bool? isOnline)
        {
            if (isOnline == null)
            {
                var paymentMethods = await _unitOfWork.PaymentMethod.GetAllAsync(e => e.ForMobile == forMobile);
                var paymentMethodResponseDTOs = _mapper.Map<IEnumerable<PaymentMethodResponseDTO>>(paymentMethods);
                return paymentMethodResponseDTOs;
            }
            else
            {
                var paymentMethods = await _unitOfWork.PaymentMethod.GetAllAsync(e => e.ForMobile == forMobile && e.IsOnline == isOnline);
                var paymentMethodResponseDTOs = _mapper.Map<IEnumerable<PaymentMethodResponseDTO>>(paymentMethods);
                return paymentMethodResponseDTOs;
            }
        }

        public async Task<IEnumerable<PaymentMethodResponseDTO>> GetAllPaymentMethod(PaginationParams? pagination)
        {
            var paymentMethods = await _unitOfWork.PaymentMethod.GetAllAsync(pagination: pagination);
            return _mapper.Map<IEnumerable<PaymentMethodResponseDTO>>(paymentMethods);
        }

        public async Task<PaymentMethodResponseDTO> GetPaymentMethod(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethod.GetFirstOrDefaultAsync(filter: e => e.Id == id);
            var paymentMethodResponseDTO = _mapper.Map<PaymentMethodResponseDTO>(paymentMethod);
            return paymentMethodResponseDTO;
        }

        public async Task<IEnumerable<PaymentMethodResponseDTO>> GetPaymentMethod(string paymentMethodName)
        {
            var paymentMethods = await _unitOfWork.PaymentMethod.GetAllAsync(e => e.MethodName.ToLower().Contains(paymentMethodName.ToLower()));
            var paymentMethodResponseDTOs = _mapper.Map<IEnumerable<PaymentMethodResponseDTO>>(paymentMethods);
            return paymentMethodResponseDTOs;
        }

        public async Task<PaymentMethodResponseDTO> UpdatePaymentMethod(int id, PaymentMethodRequestDTO paymentMethodRequestDTO)
        {
            var paymentMethod = await _unitOfWork.PaymentMethod.GetFirstOrDefaultAsync(filter: e => e.Id == id);
            if (!string.IsNullOrEmpty(paymentMethodRequestDTO.MethodName))
            {
                paymentMethod.MethodName = paymentMethodRequestDTO.MethodName;
            }
            if (paymentMethodRequestDTO.IsActive.HasValue)
            {
                paymentMethod.IsActive = paymentMethodRequestDTO.IsActive.Value;
            }
            if (!string.IsNullOrEmpty(paymentMethodRequestDTO.Description))
            {
                paymentMethod.Description = paymentMethodRequestDTO.Description;
            }
            if (paymentMethodRequestDTO.ForMobile.HasValue)
            {
                paymentMethod.ForMobile = paymentMethodRequestDTO.ForMobile.Value;
            }
            if (paymentMethodRequestDTO.IsOnline.HasValue)
            {
                paymentMethod.IsOnline = paymentMethodRequestDTO.IsOnline.Value;
            }
            _unitOfWork.PaymentMethod.Update(paymentMethod);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PaymentMethodResponseDTO>(paymentMethod);
        }
    }
}
