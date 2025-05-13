using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class MembershipCardRepository : Repository<MembershipCard>, IMembershipCardRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public MembershipCardRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(MembershipCard membershipCard)
        {
            var objFromDb = _db.MembershipCard.FirstOrDefault(s => s.Id == membershipCard.Id);
            if (objFromDb != null)
            {
                objFromDb.CustomerId = membershipCard.CustomerId;
                objFromDb.LevelMemberShipId = membershipCard.LevelMemberShipId;
                objFromDb.Points = membershipCard.Points;
                objFromDb.Description = membershipCard.Description;
                objFromDb.IsActive = membershipCard.IsActive;
            }
        }
        public async Task UpdateAsync(MembershipCard membershipCard)
        {
            var objFromDb = await _db.MembershipCard.FirstOrDefaultAsync(s => s.Id == membershipCard.Id);
            if (objFromDb != null)
            {
                objFromDb.CustomerId = membershipCard.CustomerId;
                objFromDb.LevelMemberShipId = membershipCard.LevelMemberShipId;
                objFromDb.Points = membershipCard.Points;
                objFromDb.Description = membershipCard.Description;
                objFromDb.IsActive = membershipCard.IsActive;
            }
        }
    }
}
