using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public SessionRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Session session)
        {
            var objFromDb = _db.Session.FirstOrDefault(s => s.Code == session.Code);
            objFromDb.IsActive = session.IsActive;
            objFromDb.Expired = session.Expired;

        }

        public Task UpdateAsync(Session session)
        {
            var objFromDb = _db.Session.FirstOrDefault(s => s.Code == session.Code);
            objFromDb.IsActive = session.IsActive;
            objFromDb.Expired = session.Expired;
            return Task.CompletedTask;
        }
    }
}
