using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ITypeSessionRepository : IRepository<TypeSession>
    {
        void Update(TypeSession typeSession);
        Task UpdateAsync(TypeSession typeSession);
    }
}
