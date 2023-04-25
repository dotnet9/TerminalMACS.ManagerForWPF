using System.Collections.Specialized;
using CefSharp;
using CefSharp.Handler;

namespace WpfWithCefSharpCacheDemo.Caches;

internal class CefResourceRequestHandler : ResourceRequestHandler
{
    private string _localCacheFilePath;

    private bool IsLocalCacheFileExist => System.IO.File.Exists(_localCacheFilePath);

    protected override IResourceHandler? GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser,
        IFrame frame, IRequest request)
    {
        try
        {
            _localCacheFilePath = CacheFileHelper.CalculateResourceFileName(request.Url, request.ResourceType);
            if (string.IsNullOrWhiteSpace(_localCacheFilePath))
            {
                return null;
            }
        }
        catch
        {
            return null;
        }

        if (!IsLocalCacheFileExist)
        {
            return null;
        }

        return new CefResourceHandler(_localCacheFilePath);
    }

    protected override IResponseFilter? GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser,
        IFrame frame,
        IRequest request, IResponse response)
    {
        return IsLocalCacheFileExist ? null : new CefResponseFilter { LocalCacheFilePath = _localCacheFilePath };
    }

    protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser,
        IFrame frame, IRequest request,
        IRequestCallback callback)
    {
        var headers = new NameValueCollection(request.Headers);
        headers["Authorization"] = "Bearer xxxxxx.xxxxx.xxx";
        request.Headers = headers;
        return CefReturnValue.Continue;
    }
}