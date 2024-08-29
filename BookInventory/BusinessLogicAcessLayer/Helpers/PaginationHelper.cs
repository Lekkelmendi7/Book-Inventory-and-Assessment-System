namespace BookInventory.BusinessLogicAcessLayer.Helpers
{
    public static class PaginationHelper
    {
        public static PaginatedResult<T>  Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var firstPage = 1;
            var lastPage = totalPages;

            var items = query
                .Skip((pageNumber -1)* pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<T>
            {
                TotalItems = totalItems,
                Items = items,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                HasPreviousPage = pageNumber > firstPage,
                HasNextPage = pageNumber < lastPage,
                FirstPage = firstPage,
                LastPage = lastPage,
            };
        }
    }
}
