using BussinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace DataAccess.Extensions
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context != null)
            {
                UpdateAuditFields(context);
            }
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context != null)
            {
                UpdateAuditFields(context);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditFields(DbContext context)
        {
            var now = DateTime.UtcNow;
            var userId = GetCurrentUserId();

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                
                if (entry.State == EntityState.Added)
                {
                    if (_httpContextAccessor.HttpContext != null) // chỉ khi chạy trong web request
                    {
                        if (entry.Entity.CreatedAt == default)
                            entry.Entity.CreatedAt = now;

                        if (entry.Entity.LastModified == default)
                            entry.Entity.LastModified = now;

                        entry.Entity.CreatedById = userId;
                        entry.Entity.LastModifiedById = userId;
                    }
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = now;
                    //Handle LastModifiedById
                    entry.Entity.LastModifiedById = userId;
                }
                if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.IsDeleted = true;
                    entry.Entity.LastModified = now;
                    entry.State = EntityState.Modified; // Xóa mềm
                    //Handle LastModifiedById
                    entry.Entity.LastModifiedById = userId;
                }
            }

            foreach (var entry in context.ChangeTracker.Entries<BaseEntityNoAudit>())
            {
                if (entry.State == EntityState.Added)
                {
                    if (_httpContextAccessor.HttpContext != null) // chỉ khi chạy trong web request
                    {
                        if (entry.Entity.CreatedAt == default)
                            entry.Entity.CreatedAt = now;

                        if (entry.Entity.LastModified == default)
                            entry.Entity.LastModified = now;
                    }
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = now;
                }
                if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.IsDeleted = true;
                    entry.Entity.LastModified = now;
                    entry.State = EntityState.Modified; // Xóa mềm

                }
            }
        }

        private string? GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
