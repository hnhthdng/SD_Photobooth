using BusinessLogic.DTO.PhotoStyleDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IPhotoStyleService
    {
        Task<IEnumerable<PhotoStyleResponseDTO>> GetAllPhotoStyles(PaginationParams? pagination);
        Task<IEnumerable<PhotoStyleResponseDTO>> GetPhotoStyleByName(string name);
        Task<PhotoStyleResponseDTO> GetPhotoStyleById(int id);
        Task<PhotoStyleResponseDTO> CreatePhotoStyle(PhotoStyleRequestDTO photoStyle);
        Task<PhotoStyleResponseDTO> UpdatePhotoStyle(int id, PhotoStyleRequestDTO photoStyle);
        Task<PhotoStyleResponseDTO> DeletePhotoStyle(int id);
        Task<int> GetCount();
    }
}
