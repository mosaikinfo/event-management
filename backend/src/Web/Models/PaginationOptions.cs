namespace EventManagement.WebApp.Models
{
    /// <summary>
    /// Query options for pagination, and sorting.
    /// </summary>
    public class PaginationOptions
    {
        /// <summary>
        /// Number of the page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Maximum number of items to display at a single page.
        /// </summary>
        public int PageSize { get; set; }
    }
}