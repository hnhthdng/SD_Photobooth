using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        void Update(Photo photo);
        Task UpdateAsync(Photo photo);
    }
}
