using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IStickerStyleRepository : IRepository<StickerStyle>
    {
        void Update(StickerStyle stickerStyle);
        Task UpdateAsync(StickerStyle stickerStyle);
    }
}
