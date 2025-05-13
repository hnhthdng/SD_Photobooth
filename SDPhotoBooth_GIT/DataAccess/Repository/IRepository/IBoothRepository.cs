using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IBoothRepository : IRepository<Booth>
    {
        void Update(Booth booth);
        Task UpdateAsync(Booth booth);

    }
}
