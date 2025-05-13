using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ICoordinateRepository : IRepository<Coordinate>
    {
        void Update(Coordinate booth);
        Task UpdateAsync(Coordinate booth);

    }
}
