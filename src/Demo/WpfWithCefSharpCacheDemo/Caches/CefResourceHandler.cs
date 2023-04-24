using CefSharp;
using System.IO;

namespace WpfWithCefSharpCacheDemo.Caches;

internal class CefResourceHandler : ResourceHandler
{
    public CefResourceHandler(string filePath, string mimeType = null, bool autoDisposeStream = false,
        string charset = null) : base()
    {
        if (string.IsNullOrWhiteSpace(mimeType))
        {
            var fileExtension = Path.GetExtension(filePath);
            mimeType = Cef.GetMimeType(fileExtension);
            mimeType = mimeType ?? DefaultMimeType;
        }

        var stream = File.OpenRead(filePath);
        StatusCode = 200;
        StatusText = "OK";
        MimeType = mimeType;
        Stream = stream;
        AutoDisposeStream = autoDisposeStream;
        Charset = charset;

        Headers.Add("Access-Control-Allow-Origin", "*");
    }
}