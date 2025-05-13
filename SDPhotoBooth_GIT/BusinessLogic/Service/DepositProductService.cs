using AutoMapper;
using BusinessLogic.DTO.DepositProductDTO;
using BusinessLogic.Service.IService;
using BusinessLogic.Utils;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class DepositProductService : IDepositProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepositProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> CountAsync()
        {
            return await _unitOfWork.DepositProduct.CountAsync();
        }
        public async Task<DepositProductResponseDTO> CreateAsync(DepositProductCreateRequestDTO depositProduct)
        {
            var depositProductEntity = _mapper.Map<DepositProduct>(depositProduct);
            depositProductEntity.ProductId = "";
            await _unitOfWork.DepositProduct.AddAsync(depositProductEntity);
            await _unitOfWork.SaveAsync();

            depositProductEntity.ProductId = $"product_{ProductIdHelper.Normalize(depositProductEntity.Name)}";
            await _unitOfWork.DepositProduct.UpdateAsync(depositProductEntity);
            await _unitOfWork.SaveAsync();

            var depositProductDTO = _mapper.Map<DepositProductResponseDTO>(depositProductEntity);
            return depositProductDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var depositProduct = await _unitOfWork.DepositProduct.GetFirstOrDefaultAsync(s => s.Id == id);
            if (depositProduct == null)
            {
                return false;
            }
            _unitOfWork.DepositProduct.Remove(depositProduct);
            await _unitOfWork.SaveAsync();
            return true;

        }

        public async Task<IEnumerable<DepositProductResponseDTO>> GetAllAsync(PaginationParams? pagination)
        {
            var depositProducts = await _unitOfWork.DepositProduct.GetAllAsync(pagination: pagination);
            var depositProductDTOs = _mapper.Map<IEnumerable<DepositProductResponseDTO>>(depositProducts);
            return depositProductDTOs;

        }


        public async Task<DepositProductResponseDTO> GetByIdAsync(int id)
        {
            var depositProduct = await _unitOfWork.DepositProduct.GetFirstOrDefaultAsync(s => s.Id == id);
            if (depositProduct == null)
            {
                return null;
            }
            var depositProductDTO = _mapper.Map<DepositProductResponseDTO>(depositProduct);
            return depositProductDTO;
        }

    }
}
