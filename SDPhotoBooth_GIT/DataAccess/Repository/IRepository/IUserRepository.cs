using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);
        Task UpdateAsync(User user);
    }
}
