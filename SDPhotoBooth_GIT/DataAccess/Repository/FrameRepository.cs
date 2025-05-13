using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class FrameRepository : Repository<Frame>, IFrameRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public FrameRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Frame frame)
        {
            var objFromDb = _db.Frame.FirstOrDefault(s => s.Id == frame.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = frame.Name;
                objFromDb.FrameUrl = frame.FrameUrl;
                objFromDb.FrameStyleId = frame.FrameStyleId;
                objFromDb.SlotCount = frame.SlotCount;
            }
        }

        public async Task UpdateAsync(Frame frame)
        {
            var objFromDb = await _db.Frame.FirstOrDefaultAsync(s => s.Id == frame.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = frame.Name;
                objFromDb.FrameUrl = frame.FrameUrl;
                objFromDb.FrameStyleId = frame.FrameStyleId;
                objFromDb.SlotCount = frame.SlotCount;
            }
        }
    }
}
