//namespace FCG.Application.Shared.Models
//{
//    public sealed class PagedListResponse<T>
//    {
//        public int CurrentPage { get; set; }
//        public int TotalPages { get; set; }
//        public int PageSize { get; set; }
//        public int TotalCount { get; set; }
//        public bool HasPrevious => CurrentPage > 1;
//        public bool HasNext => CurrentPage < TotalPages;
//        public List<T> Items { get; set; } = new List<T>();
//        public PagedListResponse(List<T> items, int count, int pageNumber, int pageSize)
//        {
//            TotalCount = count;
//            PageSize = pageSize;
//            CurrentPage = pageNumber;
//            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
//            Items.AddRange(items);

//        }
//    }
//}
namespace FCG.Application.Shared.Models
{
    public sealed class PagedListResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<T> Items { get; set; } = new List<T>();

        // 👇 A CORREÇÃO ESTÁ AQUI 👇
        public PagedListResponse(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage; // Agora os nomes batem!
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Items.AddRange(items);
        }
    }
}