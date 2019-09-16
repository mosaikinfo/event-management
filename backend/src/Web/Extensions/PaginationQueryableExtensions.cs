using EventManagement.WebApp.Models;
using Fop;
using Fop.FopExpression;
using System.Linq;

namespace EventManagement.WebApp.Extensions
{
    /// <summary>
    /// Extensions to <see cref="IQueryable{T}"/> to apply pagination, sorting and filtering.
    /// </summary>
    public static class PaginationQueryableExtensions
    {
        public const int DefaultPageSize = 30;

        public const int MaxPageSize = 1000;

        /// <summary>
        /// Apply pagination, sorting and filtering to the <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">entity type</typeparam>
        /// <param name="source">list of entities</param>
        /// <param name="query">query options for pagination, filtering and sorting.</param>
        public static PaginationResult<T> ApplyQuery<T>(
            this IQueryable<T> source, IFopQuery query) where T : class
        {
            query.PageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
            query.PageSize = GetPageSize(query.PageSize);

            var fopRequest = FopExpressionBuilder<Ticket>.Build(
                query.Filter, query.Order,
                query.PageNumber, query.PageSize);

            int totalCount;
            (source, totalCount) = source.ApplyFop(fopRequest);

            return new PaginationResult<T>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                Data = source.AsEnumerable(),
            };
        }

        private static int GetPageSize(int pageSize)
        {
            if (pageSize <= 0)
            {
                return DefaultPageSize;
            }
            if (pageSize > MaxPageSize)
            {
                return MaxPageSize;
            }
            return pageSize;
        }
    }
}