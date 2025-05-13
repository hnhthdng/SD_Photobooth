using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ISessionRepository : IRepository<Session>
    {
        void Update(Session session);
        Task UpdateAsync(Session session);
    }
}
