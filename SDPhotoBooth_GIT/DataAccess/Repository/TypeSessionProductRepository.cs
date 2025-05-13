using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class TypeSessionProductRepository : Repository<TypeSessionProduct>, ITypeSessionProductRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public TypeSessionProductRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(TypeSessionProduct typeSessionProduct)
        {
            var objFromDb = _db.TypeSessionProduct.FirstOrDefault(s => s.Id == typeSessionProduct.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = typeSessionProduct.Name;
                objFromDb.ProductId = typeSessionProduct.ProductId;
                objFromDb.LevelMembershipId = typeSessionProduct.LevelMembershipId;
                objFromDb.TypeSessionId = typeSessionProduct.TypeSessionId;
            }
        }
        public async Task UpdateAsync(TypeSessionProduct typeSessionProduct)
        {
            var objFromDb = await _db.TypeSessionProduct.FirstOrDefaultAsync(s => s.Id == typeSessionProduct.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = typeSessionProduct.Name;
                objFromDb.ProductId = typeSessionProduct.ProductId;
                objFromDb.LevelMembershipId = typeSessionProduct.LevelMembershipId;
                objFromDb.TypeSessionId = typeSessionProduct.TypeSessionId;
            }
        }
    }
}
