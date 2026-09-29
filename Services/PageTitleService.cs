using BookmarkManager.Common;
using BookmarkManager.Interfaces;
using HtmlAgilityPack;

namespace BookmarkManager.Services;

public class PageTitleService(
    IHttpClientFactory httpClientFactory) : IPageTitleService
{
    private readonly IHttpClientFactory _httpClientFactory =
        httpClientFactory;

    public async Task<string?> TryGetTitleAsync(
        Uri uri,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Only allow HTTP and HTTPS.
            if (!UrlValidator.IsAllowed(uri))
            {
                return null;
            }

            var client = _httpClientFactory.CreateClient("PageTitle");

            using var response = await client.GetAsync(
                uri,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var contentType = response.Content.Headers.ContentType?.MediaType;

            // We only want HTML pages.
            if (!string.Equals(
                    contentType,
                    "text/html",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var html = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (string.IsNullOrWhiteSpace(html))
            {
                return null;
            }

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var titleNode = document.DocumentNode
                .SelectSingleNode("//title");

            if (titleNode == null)
            {
                return null;
            }

            var title = HtmlEntity.DeEntitize(
                titleNode.InnerText).Trim();

            return string.IsNullOrWhiteSpace(title)
                ? null
                : title;
        }
        catch (OperationCanceledException) when (
            cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (TaskCanceledException)
        {
            return null;
        }
        catch
        {
            // Title retrieval is best-effort.
            return null;
        }
    }
}