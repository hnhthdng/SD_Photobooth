using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class TypeSessionRepository : Repository<TypeSession>, ITypeSessionRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public TypeSessionRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(TypeSession typeSession)
        {
            var objFromDb = _db.TypeSession.FirstOrDefault(s => s.Id == typeSession.Id);
            objFromDb.Name = typeSession.Name;
            objFromDb.Description = typeSession.Description;
            objFromDb.Duration = typeSession.Duration;
            objFromDb.Price = typeSession.Price;
            objFromDb.ForMobile = typeSession.ForMobile;
            objFromDb.AbleTakenNumber = typeSession.AbleTakenNumber;
        }

        public Task UpdateAsync(TypeSession typeSession)
        {
            var objFromDb = _db.TypeSession.FirstOrDefault(s => s.Id == typeSession.Id);
            objFromDb.Name = typeSession.Name;
            objFromDb.Description = typeSession.Description;
            objFromDb.Duration = typeSession.Duration;
            objFromDb.AbleTakenNumber = typeSession.AbleTakenNumber;
            objFromDb.Price = typeSession.Price;
            objFromDb.ForMobile = typeSession.ForMobile;
            return Task.CompletedTask;
        }
    }
}
