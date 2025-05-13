using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public UserRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(User user)
        {
            //implement sau
        }

        public Task UpdateAsync(User user)
        {
            //implement sau
            return Task.CompletedTask;
        }
    }
}
