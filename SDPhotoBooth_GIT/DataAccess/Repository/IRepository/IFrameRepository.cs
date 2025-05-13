using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IFrameRepository : IRepository<Frame>
    {
        void Update(Frame frame);
        Task UpdateAsync(Frame frame);
    }
}
