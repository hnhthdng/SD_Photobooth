using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class CoordinateRepository : Repository<Coordinate>, ICoordinateRepository
    {
        private readonly AIPhotoboothDbContext _context;
        public CoordinateRepository(AIPhotoboothDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Coordinate coordinate)
        {
            var objFromDb = _context.Coordinate.FirstOrDefault(s => s.Id == coordinate.Id);
            objFromDb.FrameId = coordinate.FrameId;
            objFromDb.X = coordinate.X;
            objFromDb.Y = coordinate.Y;
            objFromDb.Width = coordinate.Width;
            objFromDb.Height = coordinate.Height;
        }
        public async Task UpdateAsync(Coordinate coordinate)
        {
            var objFromDb =  await _context.Coordinate.FirstOrDefaultAsync(s => s.Id == coordinate.Id);
            objFromDb.FrameId = coordinate.FrameId;
            objFromDb.X = coordinate.X;
            objFromDb.Y = coordinate.Y;
            objFromDb.Width = coordinate.Width;
            objFromDb.Height = coordinate.Height;
        }
    }
}
