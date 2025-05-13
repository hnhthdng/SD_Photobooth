using BussinessObject.Models;

namespace DataAccess.Repository.IRepository
{
    public interface ITypeSessionProductRepository : IRepository<TypeSessionProduct>
    {
        void Update(TypeSessionProduct typeSessionProduct);
        Task UpdateAsync(TypeSessionProduct typeSessionProduct);
    }
}
