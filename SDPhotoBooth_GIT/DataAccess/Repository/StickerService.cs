using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class StickerRepository : Repository<Sticker>, IStickerRepository
    {
        private readonly AIPhotoboothDbContext _context;
        public StickerRepository(AIPhotoboothDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Sticker sticker)
        {
            var objFromDb = _context.Sticker.FirstOrDefault(s => s.Id == sticker.Id);
            objFromDb.Name = sticker.Name;
            objFromDb.StickerUrl = sticker.StickerUrl;
            objFromDb.StickerStyleId = sticker.StickerStyleId;

        }

        public Task UpdateAsync(Sticker sticker)
        {
            var objFromDb = _context.Sticker.FirstOrDefault(s => s.Id == sticker.Id);
            objFromDb.Name = sticker.Name;
            objFromDb.StickerUrl = sticker.StickerUrl;
            objFromDb.StickerStyleId = sticker.StickerStyleId;
            return Task.CompletedTask;
        }
    }
}
