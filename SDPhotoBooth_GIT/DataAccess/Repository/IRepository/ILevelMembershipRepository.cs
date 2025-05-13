using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ILevelMembershipRepository : IRepository<LevelMembership>
    {
        void Update(LevelMembership obj);
        Task UpdateAsync(LevelMembership obj);
    }
}
