using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IPhotoStyleRepository : IRepository<PhotoStyle>
    {
        void Update(PhotoStyle photoStyle);
        Task UpdateAsync(PhotoStyle photoStyle);
    }
}
