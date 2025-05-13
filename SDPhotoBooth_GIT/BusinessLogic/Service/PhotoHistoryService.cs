using AutoMapper;
using BusinessLogic.DTO.PhotoDTO;
using BusinessLogic.DTO.PhotoHistoryDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Service
{
    public class PhotoHistoryService : IPhotoHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PhotoHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
           return await _unitOfWork.PhotoHistory.CountAsync();
        }

        public async Task<string[]> GetListPhotos(int sessionId)
        {
            var photoHistory = await _unitOfWork.PhotoHistory.GetFirstOrDefaultAsync(includeProperties: "Photos", filter: o => o.SessionId == sessionId);
            if (photoHistory == null)
            {
                return null;
            }

            return photoHistory.Photos.Select(p => p.Url).ToArray();
        }

        public async Task<string[]> GetListPhotosFromSessionCode(string sessionCode)
        {
            var session = await _unitOfWork.Session.GetFirstOrDefaultAsync(filter: o => o.Code == sessionCode);
            var photoHistory = await _unitOfWork.PhotoHistory.GetFirstOrDefaultAsync(includeProperties: "Photos", filter: o => o.SessionId == session.Id);
            if (photoHistory == null)
            {
                return null;
            }

            return photoHistory.Photos.Select(p => p.Url).ToArray();
        }

        public async Task<PhotoHistoryResponseDTO> GetPhotoHistoryAsync(int id, string userId)
        {
            var photoHistory = await _unitOfWork.PhotoHistory.GetFirstOrDefaultAsync(includeProperties: "Photos,Photos.PhotoStyle", filter: o => o.Id == id && o.CustomerId == userId);
            if (photoHistory == null)
            {
                return null;
            }

            var photoHistoryResponseDTO = _mapper.Map<PhotoHistoryResponseDTO>(photoHistory);
            return photoHistoryResponseDTO;
        }

        public async Task<IEnumerable<PhotoHistoryResponseDTO>> GetListPhotoHistoryAsync(string userId)
        {
            var photoHistories = await _unitOfWork.PhotoHistory.GetAllAsync(filter: o => o.CustomerId == userId);
            return photoHistories.Select(photoHistory => new PhotoHistoryResponseDTO
            {
                Id = photoHistory.Id,
                CreatedAt = photoHistory.CreatedAt,
                UpdatedAt = photoHistory.LastModified
            });
        }
        public async Task<IEnumerable<PhotoHistoryResponseDTO>> GetAllPhotoHistory(PaginationParams? pagination)
        {
            var photoHistories = await _unitOfWork.PhotoHistory.GetAllAsync(
                pagination: pagination
            );

            return _mapper.Map<IEnumerable<PhotoHistoryResponseDTO>>(photoHistories);
        }

        public async Task<IEnumerable<PhotoHistoryResponseDTO>> GetPhotoHistoryByCustomerIdAsync(string customerId)
        {
            var photoHistories = await _unitOfWork.PhotoHistory.GetAllAsync(filter: o => o.CustomerId == customerId);
            var photoHistoriesResponseDTO = _mapper.Map<IEnumerable<PhotoHistoryResponseDTO>>(photoHistories);
            return photoHistoriesResponseDTO;
        }
        public async Task<IEnumerable<PhotoHistoryResponseDTO>> GetPhotoHistoryByBoothIdAsync(int boothId)
        {
            var photoHistories = await _unitOfWork.PhotoHistory.GetAllAsync(filter: o => o.BoothId == boothId);
            var photoHistoriesResponseDTO = _mapper.Map<IEnumerable<PhotoHistoryResponseDTO>>(photoHistories);
            return photoHistoriesResponseDTO;
        }
        public async Task<IEnumerable<PhotoHistoryResponseDTO>> GetPhotoHistoryByLocationIdAsync(int locationId)
        {
            var photoHistories = await _unitOfWork.PhotoHistory.GetAllAsync(includeProperties: "Booth,Booth.Location", filter: o => o.Booth.LocationId == locationId);
            var photoHistoriesResponseDTO = _mapper.Map<IEnumerable<PhotoHistoryResponseDTO>>(photoHistories);
            return photoHistoriesResponseDTO;
        }

        

        public async Task<bool> SaveUploadedPhotos(string sessionCode, List<string> photoUrls, int PhotoStyleId)
        {
            var session = await _unitOfWork.Session.GetFirstOrDefaultAsync(filter: o => o.Code == sessionCode);
            if (session == null)
            {
                return false;
            }

            var photoHistory = await _unitOfWork.PhotoHistory.GetFirstOrDefaultAsync(
                filter: o => o.SessionId == session.Id,
                includeProperties: "Photos"
            );

            if (photoHistory == null)
            {
                return false;
            }

            var existingUrls = photoHistory.Photos.Select(p => p.Url).ToHashSet();

            foreach (var url in photoUrls)
            {
                if (!existingUrls.Contains(url))
                {
                    photoHistory.Photos.Add(new Photo
                    {
                        Url = url,
                        PhotoHistoryId = photoHistory.Id,
                        PhotoStyleId = PhotoStyleId
                    });
                }
            }

            _unitOfWork.PhotoHistory.Update(photoHistory);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PhotoResponseDTO>> GetPhotoHistoryByIdAsync(int photoHistoryId)
        {
            var photoHistory =await _unitOfWork.PhotoHistory.GetFirstOrDefaultAsync(
                filter: o => o.Id == photoHistoryId,
                includeProperties: "Photos,Photos.PhotoStyle"
            );
            if (photoHistory == null)
            {
                return null;
            }
            var photoResponseDTOs = new List<PhotoResponseDTO>();
            foreach (var photo in photoHistory.Photos)
            {
                photoResponseDTOs.Add(new PhotoResponseDTO
                {
                    Url = photo.Url,
                    PhotoStyleName = photo.PhotoStyle.Name
                });
            }
            return photoResponseDTOs;
        }

        public async Task<IEnumerable<PhotoResponseDTO>> GetPhotoHistoryBySessionCodeAsync(string sessionCode)
        {
            var session = await _unitOfWork.Session.GetFirstOrDefaultAsync(
                s => EF.Functions.Collate(s.Code, "Latin1_General_CS_AS") == sessionCode
            );
            if (session == null)
            {
                return null;
            }

            var photoHistory = await _unitOfWork.PhotoHistory.GetFirstOrDefaultAsync(
                includeProperties: "Session,Photos,Photos.PhotoStyle", filter: o => o.Session.Code == sessionCode);

            if (photoHistory == null)
            {
                return null;
            }
            var photoHistoryResponseDTO = _mapper.Map<PhotoHistoryResponseDTO>(photoHistory);

            var photoResponseDTOs = new List<PhotoResponseDTO>();
            foreach (var photo in photoHistory.Photos)
            {
                photoResponseDTOs.Add(new PhotoResponseDTO
                {
                    Url = photo.Url,
                    PhotoStyleName = photo.PhotoStyle.Name
                });
            }
            return photoResponseDTOs;
        }
    }
}
