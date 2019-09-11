using System.Collections.Generic;

namespace EventManagement.WebApp.Models
{
    public class PaginationResult<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}