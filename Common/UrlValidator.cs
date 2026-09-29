using System.Net;

namespace BookmarkManager.Common;

public static class UrlValidator
{
    public static bool IsAllowed(Uri uri)
    {
        if (uri.Scheme != Uri.UriSchemeHttp &&
            uri.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(uri.Host))
        {
            return false;
        }

        if (IPAddress.TryParse(uri.Host, out var ipAddress))
        {
            return !IPAddress.IsLoopback(ipAddress) &&
                   !ipAddress.IsIPv6LinkLocal &&
                   !IsPrivate(ipAddress);
        }

        return !string.Equals(
            uri.Host,
            "localhost",
            StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsPrivate(IPAddress ip)
    {
        if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return false;
        }

        var bytes = ip.GetAddressBytes();

        return
            bytes[0] == 10 ||
            (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
            (bytes[0] == 192 && bytes[1] == 168);
    }
}