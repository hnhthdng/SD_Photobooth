using DataAccess.Data;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AIPhotoboothDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(AIPhotoboothDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        // Thêm mới một thực thể
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await dbSet.AddAsync(entity, cancellationToken);
        }

        // Thêm nhiều thực thể
        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public int Count(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                query = query.Where(entity => !EF.Property<bool>(entity, "IsDeleted"));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = dbSet;

            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                query = query.Where(entity => !EF.Property<bool>(entity, "IsDeleted"));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync(cancellationToken);
        }


        public IEnumerable<T> GetAll(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null,
        bool asNoTracking = false,
        PaginationParams? pagination = null)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                query = query.Where(entity => !EF.Property<bool>(entity, "IsDeleted"));
            }

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            if (orderBy != null)
                query = orderBy(query);

            if (pagination != null)
            {
                var (isPaged, pageNumber, pageSize) = pagination.Validate();
                if (isPaged)
                {
                    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null,
        bool asNoTracking = false,
        PaginationParams? pagination = null,
        CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (typeof(T).GetProperty("IsDeleted") != null)
                query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProp.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            if (pagination != null)
            {
                var (isPaged, pageNumber, pageSize) = pagination.Validate();
                if (isPaged)
                    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            return await query.ToListAsync(cancellationToken);
        }



        public T GetFirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking) query = query.AsNoTracking();
            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                query = query.Where(entity => !EF.Property<bool>(entity, "IsDeleted"));
            }
            if (filter != null) query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public async Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking) query = query.AsNoTracking();
            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                query = query.Where(entity => !EF.Property<bool>(entity, "IsDeleted"));
            }
            if (filter != null) query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Remove(int id)
        {
            var entity = dbSet.Find(id);
            if (entity != null) Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
