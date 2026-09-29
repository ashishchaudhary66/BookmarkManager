namespace BookmarkManager.Models.Dtos
{
    public class BookmarkQuery
    {
        public string? Search { get; set; }
        public string? Tag { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public void Normalize()
        {
            Page = Math.Max(Page, 1);
            PageSize = Math.Clamp(PageSize, 1, 100);

            Search = Search?.Trim();
            Tag = Tag?.Trim();
        }
    }
}
