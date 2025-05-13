using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class PhotoHistoryRepository : Repository<PhotoHistory>, IPhotoHistoryRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public PhotoHistoryRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(PhotoHistory photoHistory)
        {
            var objFromDb = _db.PhotoHistory.FirstOrDefault(s => s.Id == photoHistory.Id);
            objFromDb.CustomerId = photoHistory.CustomerId;
            objFromDb.SessionId = photoHistory.SessionId;
        }

        public Task UpdateAsync(PhotoHistory photoHistory)
        {
            var objFromDb = _db.PhotoHistory.FirstOrDefault(s => s.Id == photoHistory.Id);
            objFromDb.CustomerId = photoHistory.CustomerId;
            objFromDb.SessionId = photoHistory.SessionId;
            return Task.CompletedTask;
        }
    }
}
