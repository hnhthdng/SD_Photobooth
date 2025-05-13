using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IPhotoHistoryRepository : IRepository<PhotoHistory>
    {
        void Update(PhotoHistory photoHistory);
        Task UpdateAsync(PhotoHistory photoHistory);
    }
}
