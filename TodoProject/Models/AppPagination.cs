using MongoDB.Driver;

namespace TodoProject.Models
{
    public class AppPagination<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public long TotalItems { get; set; }
        public List<T> Items { get; set; } = new List<T>();

        public AppPagination(int page, int pageSize, long totalItems, List<T> items)
        {
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            TotalItems = totalItems;
            Items = items;
        }

        public static async Task<AppPagination<T>> HandlePagination(IFindFluent<T, T> findFluent, int page, int pageSize)
        {
            var totalItems = await findFluent.CountAsync();
            int skipResult = (page - 1) * pageSize;
            var items = await findFluent.Skip(skipResult).Limit(pageSize).ToListAsync();
            return new AppPagination<T>(page, pageSize, totalItems, items);
        }
    }
}
