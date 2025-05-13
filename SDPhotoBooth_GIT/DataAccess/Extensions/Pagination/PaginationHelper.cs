namespace DataAccess.Extensions.Pagination
{
    public static class PaginationHelper
    {
        public static (bool IsPaged, int PageNumber, int PageSize) Validate(this PaginationParams? param)
        {
            if (param == null || (!param.PageNumber.HasValue && !param.PageSize.HasValue))
                return (false, 0, 0);

            if (param.PageNumber.HasValue && !param.PageSize.HasValue)
                return (true, param.PageNumber.Value, 10); // default pageSize

            if (!param.PageNumber.HasValue && param.PageSize.HasValue)
                throw new ArgumentException("If you provide pageSize, you must also provide pageNumber.");

            return (true, param.PageNumber.Value, param.PageSize.Value);
        }
    }
}
    
