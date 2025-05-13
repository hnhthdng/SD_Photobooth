using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ServerAPI.Filters
{
    public class TransactionFilter : IAsyncActionFilter
    {
        private readonly AIPhotoboothDbContext _dbContext;

        public TransactionFilter(AIPhotoboothDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            var executedContext = await next();

            if (executedContext.Exception != null && !executedContext.ExceptionHandled)
            {
                await transaction.RollbackAsync();
                return;
            }

            int? resultStatusCode = executedContext.Result switch
            {
                ObjectResult objectResult => objectResult.StatusCode,
                StatusCodeResult statusCodeResult => statusCodeResult.StatusCode,
                _ => null
            };

            if (resultStatusCode.HasValue && resultStatusCode.Value >= 400)
            {
                await transaction.RollbackAsync();
                return;
            }

            await transaction.CommitAsync();
        }

    }

}
