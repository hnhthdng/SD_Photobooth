using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class StickerStyleRepository : Repository<StickerStyle>, IStickerStyleRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public StickerStyleRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(StickerStyle stickerStyle)
        {
            var objFromDb = _db.StickerStyle.FirstOrDefault(s => s.Id == stickerStyle.Id);
            if (objFromDb != null)
            {
                objFromDb.StickerStyleName = stickerStyle.StickerStyleName;
                objFromDb.Description = stickerStyle.Description;
            }
        }
        public async Task UpdateAsync(StickerStyle stickerStyle)
        {
            var objFromDb = await _db.StickerStyle.FirstOrDefaultAsync(s => s.Id == stickerStyle.Id);
            if (objFromDb != null)
            {
                objFromDb.StickerStyleName = stickerStyle.StickerStyleName;
                objFromDb.Description = stickerStyle.Description;
            }
        }
    }
}
