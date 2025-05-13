using BusinessLogic.DTO.PhotoDTO;
using BusinessLogic.DTO.PhotoHistoryDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IPhotoHistoryService
    {
        Task<string[]> GetListPhotos(int sessionId);
        Task<string[]> GetListPhotosFromSessionCode(string sessionCode);
        Task<IEnumerable<PhotoHistoryResponseDTO>> GetListPhotoHistoryAsync(string userId);
        Task<PhotoHistoryResponseDTO> GetPhotoHistoryAsync(int id, string userId);

        //Managing in Web
        Task<IEnumerable<PhotoHistoryResponseDTO>> GetAllPhotoHistory(PaginationParams? pagination);
        Task<IEnumerable<PhotoHistoryResponseDTO>> GetPhotoHistoryByCustomerIdAsync(string customerId);
        Task<IEnumerable<PhotoHistoryResponseDTO>> GetPhotoHistoryByBoothIdAsync(int boothId);
        Task<IEnumerable<PhotoHistoryResponseDTO>> GetPhotoHistoryByLocationIdAsync(int locationId);
        Task<IEnumerable<PhotoResponseDTO>> GetPhotoHistoryBySessionCodeAsync(string sessionCode);
        Task<bool> SaveUploadedPhotos(string sessionCode, List<string> photoUrls, int PhotoStyleId = 1);
        Task<int> GetCount();
        Task<IEnumerable<PhotoResponseDTO>> GetPhotoHistoryByIdAsync(int photoHistoryId);
    }
}