using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class FrameStyleRepository : Repository<FrameStyle>, IFrameStyleRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public FrameStyleRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(FrameStyle frameStyle)
        {
            var objFromDb = _db.FrameStyle.FirstOrDefault(s => s.Id == frameStyle.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = frameStyle.Name;
                objFromDb.Description = frameStyle.Description;
                objFromDb.ImageUrl = frameStyle.ImageUrl;
            }
        }
        public async Task UpdateAsync(FrameStyle frameStyle)
        {
            var objFromDb = await _db.FrameStyle.FirstOrDefaultAsync(s => s.Id == frameStyle.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = frameStyle.Name;
                objFromDb.Description = frameStyle.Description;
                objFromDb.ImageUrl = frameStyle.ImageUrl;
            }
        }
    }
}
