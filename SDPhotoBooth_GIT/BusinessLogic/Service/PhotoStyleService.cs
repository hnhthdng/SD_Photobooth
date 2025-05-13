using AutoMapper;
using BusinessLogic.DTO.PhotoStyleDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class PhotoStyleService : IPhotoStyleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PhotoStyleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            var photoStyles = await _unitOfWork.PhotoStyle.GetAllAsync();
            return photoStyles.Count();
        }
        public async Task<PhotoStyleResponseDTO> CreatePhotoStyle(PhotoStyleRequestDTO photoStyle)
        {
            var isExist = await _unitOfWork.PhotoStyle.GetFirstOrDefaultAsync(t => t.Name == photoStyle.Name && !t.IsDeleted);
            if (isExist != null)
            {
                return null;
            }
            else
            {
                var photoStyleObj = _mapper.Map<PhotoStyle>(photoStyle);
                await _unitOfWork.PhotoStyle.AddAsync(photoStyleObj);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<PhotoStyleResponseDTO>(photoStyleObj);
            }
        }
        public async Task<PhotoStyleResponseDTO> DeletePhotoStyle(int id)
        {
            var photoStyle = await _unitOfWork.PhotoStyle.GetFirstOrDefaultAsync(t => t.Id == id);
            if (photoStyle == null)
            {
                return null;
            }
            _unitOfWork.PhotoStyle.Remove(photoStyle);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PhotoStyleResponseDTO>(photoStyle);
        }

        public async Task<IEnumerable<PhotoStyleResponseDTO>> GetAllPhotoStyles(PaginationParams? pagination)
        {
            var photoStyles = await _unitOfWork.PhotoStyle.GetAllAsync(pagination: pagination);
            return _mapper.Map<IEnumerable<PhotoStyleResponseDTO>>(photoStyles);
        }


        public async Task<PhotoStyleResponseDTO> GetPhotoStyleById(int id)
        {
            var photoStyle = await _unitOfWork.PhotoStyle.GetFirstOrDefaultAsync(t => t.Id == id);
            if (photoStyle == null)
            {
                return null;
            }
            return _mapper.Map<PhotoStyleResponseDTO>(photoStyle);
        }

        public async Task<IEnumerable<PhotoStyleResponseDTO>> GetPhotoStyleByName(string name)
        {
            var photoStyles = await _unitOfWork.PhotoStyle.GetAllAsync(t => t.Name.ToLower().Contains(name.ToLower()));
            if (photoStyles == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<PhotoStyleResponseDTO>>(photoStyles);
        }

        public async Task<PhotoStyleResponseDTO> UpdatePhotoStyle(int id, PhotoStyleRequestDTO photoStyle)
        {
            var isExistPhotoName = await _unitOfWork.PhotoStyle.GetFirstOrDefaultAsync(t => t.Name == photoStyle.Name && !t.IsDeleted && t.Id != id);
            if (isExistPhotoName != null)
            {
                return null;
            }
            var photoStyleObj = await _unitOfWork.PhotoStyle.GetFirstOrDefaultAsync(t => t.Id == id);
            if(!string.IsNullOrEmpty(photoStyle.Name))
            {
                photoStyleObj.Name = photoStyle.Name;
            }
            if (!string.IsNullOrEmpty(photoStyle.Description))
            {
                photoStyleObj.Description = photoStyle.Description;
            }
            if (!string.IsNullOrEmpty(photoStyle.ImageUrl))
            {
                photoStyleObj.ImageUrl = photoStyle.ImageUrl;
            }
            if (!string.IsNullOrEmpty(photoStyle.Prompt))
            {
                photoStyleObj.Prompt = photoStyle.Prompt;
            }
            if (!string.IsNullOrEmpty(photoStyle.NegativePrompt))
            {
                photoStyleObj.NegativePrompt = photoStyle.NegativePrompt;
            }
            if (!string.IsNullOrEmpty(photoStyle.Controlnets))
            {
                photoStyleObj.Controlnets = photoStyle.Controlnets;
            }
            if (photoStyle.NumImagesPerGen != null)
            {
                photoStyleObj.NumImagesPerGen = photoStyle.NumImagesPerGen;
            }
            if (!string.IsNullOrEmpty(photoStyle.BackgroundColor))
            {
                photoStyleObj.BackgroundColor = photoStyle.BackgroundColor;
            }
            if (photoStyle.Height != null)
            {
                photoStyleObj.Height = photoStyle.Height;
            }
            if (photoStyle.Width != null)
            {
                photoStyleObj.Width = photoStyle.Width;
            }
            if (photoStyle.Mode != null)
            {
                photoStyleObj.Mode = photoStyle.Mode;
            }
            if (photoStyle.NumInferenceSteps != null)
            {
                photoStyleObj.NumInferenceSteps = photoStyle.NumInferenceSteps;
            }
            if (photoStyle.GuidanceScale != null)
            {
                photoStyleObj.GuidanceScale = photoStyle.GuidanceScale;
            }
            if (photoStyle.Strength != null)
            {
                photoStyleObj.Strength = photoStyle.Strength;
            }
            if (photoStyle.IPAdapterScale != null)
            {
                photoStyleObj.IPAdapterScale = photoStyle.IPAdapterScale;
            }
            if (photoStyle.BackgroundRemover != null)
            {
                photoStyleObj.BackgroundRemover = photoStyle.BackgroundRemover;
            }

            try
            {
                _unitOfWork.PhotoStyle.Update(photoStyleObj);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<PhotoStyleResponseDTO>(photoStyleObj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    } 
}
