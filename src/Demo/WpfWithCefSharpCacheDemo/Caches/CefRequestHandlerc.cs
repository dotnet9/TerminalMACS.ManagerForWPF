﻿using CefSharp;
using CefSharp.Handler;

namespace WpfWithCefSharpCacheDemo.Caches;

internal class CefRequestHandlerc : RequestHandler
{
    protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
        IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
    {
        // 一个请求用一个CefResourceRequestHandler
        return new CefResourceRequestHandler();
    }
}