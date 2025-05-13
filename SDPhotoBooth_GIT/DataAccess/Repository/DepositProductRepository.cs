using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class DepositProductRepository : Repository<DepositProduct>, IDepositProductRepository
    {
        private readonly AIPhotoboothDbContext _context;
        public DepositProductRepository(AIPhotoboothDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(DepositProduct depositProduct)
        {
            var objFromDb = _context.DepositProduct.FirstOrDefault(s => s.Id == depositProduct.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = depositProduct.Name;
                objFromDb.Description = depositProduct.Description;
                objFromDb.ProductId = depositProduct.ProductId;
                objFromDb.Price = depositProduct.Price;
                objFromDb.AmountAdd = depositProduct.AmountAdd;
                _context.SaveChanges();
            }
        }

        public async Task UpdateAsync(DepositProduct depositProduct)
        {
            var objFromDb = await _context.DepositProduct.FirstOrDefaultAsync(s => s.Id == depositProduct.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = depositProduct.Name;
                objFromDb.Description = depositProduct.Description;
                objFromDb.ProductId = depositProduct.ProductId;
                objFromDb.Price = depositProduct.Price;
                objFromDb.AmountAdd = depositProduct.AmountAdd;
                await _context.SaveChangesAsync();
            }
        }
    }
}
