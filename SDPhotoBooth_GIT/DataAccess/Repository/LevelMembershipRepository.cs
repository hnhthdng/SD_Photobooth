using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class LevelMembershipRepository : Repository<LevelMembership>, ILevelMembershipRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public LevelMembershipRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(LevelMembership obj)
        {
            var levelMembership = _db.LevelMembership.FirstOrDefault(l => l.Id == obj.Id);
            if (levelMembership != null)
            {
                levelMembership.Name = obj.Name;
                levelMembership.Description = obj.Description;
                levelMembership.Point = obj.Point;
                levelMembership.IsActive = obj.IsActive;
                levelMembership.DiscountPercent = obj.DiscountPercent;
                levelMembership.MaxDiscount = obj.MaxDiscount;
                levelMembership.MinOrder = obj.MinOrder;
                levelMembership.NextLevelId = obj.NextLevelId;
            }
        }
        public async Task UpdateAsync(LevelMembership obj)
        {
            var levelMembership = await _db.LevelMembership.FirstOrDefaultAsync(l => l.Id == obj.Id);
            if (levelMembership != null)
            {
                levelMembership.Name = obj.Name;
                levelMembership.Description = obj.Description;
                levelMembership.Point = obj.Point;
                levelMembership.IsActive = obj.IsActive;
                levelMembership.DiscountPercent = obj.DiscountPercent;
                levelMembership.MaxDiscount = obj.MaxDiscount;
                levelMembership.MinOrder = obj.MinOrder;
                levelMembership.NextLevelId = obj.NextLevelId;
            }
        }
    }
}
