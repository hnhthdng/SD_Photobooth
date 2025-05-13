using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface IFrameStyleRepository : IRepository<FrameStyle>
    {
        void Update(FrameStyle frameStyle);
        Task UpdateAsync(FrameStyle frameStyle);
    }
}
