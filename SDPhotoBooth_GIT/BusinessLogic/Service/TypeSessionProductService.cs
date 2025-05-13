using AutoMapper;
using BusinessLogic.DTO.TypeSessionProductDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class TypeSessionProductService : ITypeSessionProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TypeSessionProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CountAsync()
        {
            return await _unitOfWork.TypeSessionProduct.CountAsync();
        }
        public async Task<TypeSessionProductResponseDTO> CreateAsync(TypeSessionProductCreateRequestDTO typeSessionProduct)
        {
            var typeSessionProductEntity = _mapper.Map<TypeSessionProduct>(typeSessionProduct);
            await _unitOfWork.TypeSessionProduct.AddAsync(typeSessionProductEntity);
            await _unitOfWork.SaveAsync();
            var typeSessionProductDTO = _mapper.Map<TypeSessionProductResponseDTO>(typeSessionProductEntity);
            return typeSessionProductDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var typeSessionProduct = await _unitOfWork.TypeSessionProduct.GetFirstOrDefaultAsync(s => s.Id == id);
            if (typeSessionProduct == null)
            {
                return false;
            }
            typeSessionProduct.IsDeleted = true;
            await _unitOfWork.TypeSessionProduct.UpdateAsync(typeSessionProduct);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<TypeSessionProductResponseDTO>> GetAllAsync(PaginationParams? pagination)
        {
            var typeSessionProducts = await _unitOfWork.TypeSessionProduct.GetAllAsync(pagination: pagination);
            var typeSessionProductDTOs = _mapper.Map<IEnumerable<TypeSessionProductResponseDTO>>(typeSessionProducts);
            return typeSessionProductDTOs;
        }

        public async Task<IEnumerable<TypeSessionProductResponseDTO>> GetAllIncludeAsync()
        {
            var typeSessionProducts = await _unitOfWork.TypeSessionProduct.GetAllAsync(includeProperties: "LevelMembership,TypeSession,Coupon");
            var typeSessionProductDTOs = _mapper.Map<IEnumerable<TypeSessionProductResponseDTO>>(typeSessionProducts);
            return typeSessionProductDTOs;
        }

        public async Task<IEnumerable<TypeSessionProductResponseDTO>> GetByCouponAndTypeSessionIdAsync(int couponId, int typeSessionId)
        {
            var typeSessionProducts = await _unitOfWork.TypeSessionProduct.GetAllAsync(s => s.CouponId == couponId && s.TypeSessionId == typeSessionId, includeProperties: "Coupon");
            var typeSessionProductDTOs = _mapper.Map<IEnumerable<TypeSessionProductResponseDTO>>(typeSessionProducts);
            return typeSessionProductDTOs;
        }

        public async Task<TypeSessionProductResponseDTO> GetByIdAsync(int id)
        {
            var typeSessionProduct = await _unitOfWork.TypeSessionProduct.GetFirstOrDefaultAsync(s => s.Id == id);
            if (typeSessionProduct == null)
            {
                return null;
            }
            var typeSessionProductDTO = _mapper.Map<TypeSessionProductResponseDTO>(typeSessionProduct);
            return typeSessionProductDTO;
        }

        public  async Task<IEnumerable<TypeSessionProductResponseDTO>> GetByLevelMembershipIdAsync(int levelMembershipId)
        {
            var typeSessionProducts = await _unitOfWork.TypeSessionProduct.GetAllAsync(s => s.LevelMembershipId == levelMembershipId, includeProperties: "LevelMembership,TypeSession");
            var typeSessionProductDTOs = _mapper.Map<IEnumerable<TypeSessionProductResponseDTO>>(typeSessionProducts);
            return typeSessionProductDTOs;
        }

        public async Task<IEnumerable<TypeSessionProductResponseDTO>> GetByProductIdAsync(string productId)
        {
            var typeSessionProducts = await _unitOfWork.TypeSessionProduct.GetAllAsync(s => s.ProductId == productId);
            var typeSessionProductDTOs = _mapper.Map<IEnumerable<TypeSessionProductResponseDTO>>(typeSessionProducts);
            return typeSessionProductDTOs;
        }

        public async Task<IEnumerable<TypeSessionProductResponseDTO>> GetByTypeSessionIdAsync(int typeSessionId)
        {
            var typeSessionProducts = await _unitOfWork.TypeSessionProduct.GetAllAsync(s => s.TypeSessionId == typeSessionId);
            var typeSessionProductDTOs = _mapper.Map<IEnumerable<TypeSessionProductResponseDTO>>(typeSessionProducts);
            return typeSessionProductDTOs;
        }

        public async Task<TypeSessionProductResponseDTO> UpdateAsync(int id, TypeSessionProductUpdateRequestDTO typeSessionProduct)
        {
            var typeSessionProductEntity = await _unitOfWork.TypeSessionProduct.GetFirstOrDefaultAsync(s => s.Id == id);

            if (!string.IsNullOrEmpty(typeSessionProduct.Name))
            {
                typeSessionProductEntity.Name = typeSessionProduct.Name;
            }
            if (!string.IsNullOrEmpty(typeSessionProduct.ProductId))
            {
                typeSessionProductEntity.ProductId = typeSessionProduct.ProductId;
            }

            await _unitOfWork.TypeSessionProduct.UpdateAsync(typeSessionProductEntity);
            await _unitOfWork.SaveAsync();
            var typeSessionProductDTO = _mapper.Map<TypeSessionProductResponseDTO>(typeSessionProductEntity);
            return typeSessionProductDTO;
        }
    }
}
