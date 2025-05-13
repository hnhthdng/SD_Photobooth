using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public LocationRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Location location)
        {
            var objFromDb = _db.Location.FirstOrDefault(s => s.Id == location.Id);
            objFromDb.LocationName = location.LocationName;
            objFromDb.Address = location.Address;
        }

        public Task UpdateAsync(Location location)
        {
            var objFromDb = _db.Location.FirstOrDefault(s => s.Id == location.Id);
            objFromDb.LocationName = location.LocationName;
            objFromDb.Address = location.Address;
            return Task.CompletedTask;
        }
    }
}
