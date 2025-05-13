using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IMembershipCardRepository : IRepository<MembershipCard>
    {
        void Update(MembershipCard membershipCard);
        Task UpdateAsync(MembershipCard membershipCard);
    }
}
