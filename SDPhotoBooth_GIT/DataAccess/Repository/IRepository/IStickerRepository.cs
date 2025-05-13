using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IStickerRepository : IRepository<Sticker>
    {
        void Update(Sticker sticker);
        Task UpdateAsync(Sticker sticker);
    }
}
