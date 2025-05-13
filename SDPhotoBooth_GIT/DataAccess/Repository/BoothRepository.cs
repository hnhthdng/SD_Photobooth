using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class BoothRepository : Repository<Booth>, IBoothRepository
    {
        private readonly AIPhotoboothDbContext _context;
        public BoothRepository(AIPhotoboothDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Booth booth)
        {
            var objFromDb = _context.Booth.FirstOrDefault(s => s.Id == booth.Id);
            objFromDb.LocationId = booth.LocationId;
            objFromDb.BoothName = booth.BoothName;
            objFromDb.Description = booth.Description;
            objFromDb.Status = booth.Status;
        }

        public Task UpdateAsync(Booth booth)
        {
            var objFromDb = _context.Booth.FirstOrDefault(s => s.Id == booth.Id);
            objFromDb.LocationId = booth.LocationId;
            objFromDb.BoothName = booth.BoothName;
            objFromDb.Description = booth.Description;
            objFromDb.Status = booth.Status;
            return Task.CompletedTask;
        }
    }
}
