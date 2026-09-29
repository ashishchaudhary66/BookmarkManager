namespace BookmarkManager.Models.Dtos
{
    public class BookmarkQuery
    {
        public string? Search { get; set; }
        public string? Tag { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
