using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public PhotoRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Photo photo)
        {
            var objFromDb = _db.Photo.FirstOrDefault(s => s.Id == photo.Id);
            objFromDb.PhotoHistoryId = photo.PhotoHistoryId;
            objFromDb.Url = photo.Url;
            objFromDb.PhotoStyleId = photo.PhotoStyleId;
        }

        public Task UpdateAsync(Photo photo)
        {
            var objFromDb = _db.Photo.FirstOrDefault(s => s.Id == photo.Id);
            objFromDb.PhotoHistoryId = photo.PhotoHistoryId;
            objFromDb.Url = photo.Url;
            objFromDb.PhotoStyleId = photo.PhotoStyleId;
            return Task.CompletedTask;
        }
    }
}
