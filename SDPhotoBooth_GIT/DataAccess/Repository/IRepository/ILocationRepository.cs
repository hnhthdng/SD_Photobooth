using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ILocationRepository : IRepository<Location>
    {
        void Update(Location location); 
        Task UpdateAsync(Location location);
    }
}
