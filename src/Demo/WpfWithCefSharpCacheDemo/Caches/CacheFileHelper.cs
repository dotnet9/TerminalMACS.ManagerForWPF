using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace WpfWithCefSharpCacheDemo.Caches;

internal static class CacheFileHelper
{
    private const string DEV_TOOLS_SCHEME = "devtools";
    private const string DEFAULT_INDEX_FILE = "index.html";

    private static HashSet<string> needInterceptedAjaxInterfaces = new();

    private static string CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "caches");

    public static void AddInterceptedAjaxInterfaces(string url)
    {
        if (needInterceptedAjaxInterfaces.Contains(url))
        {
            return;
        }

        needInterceptedAjaxInterfaces.Add(url);
    }

    private static bool IsNeedInterceptedAjaxInterface(string url, ResourceType resourceType)
    {
        var uri = new Uri(url);
        if (DEV_TOOLS_SCHEME == url)
        {
            return false;
        }

        if (ResourceType.Xhr == resourceType && !needInterceptedAjaxInterfaces.Contains(url))
        {
            return false;
        }

        return true;
    }

    public static string? CalculateResourceFileName(string url, ResourceType resourceType)
    {
        if (!IsNeedInterceptedAjaxInterface(url, resourceType))
        {
            return default;
        }

        var uri = new Uri(url);
        var urlPath = uri.LocalPath;

        if (urlPath.StartsWith("/"))
        {
            urlPath = urlPath.Substring(1);
        }

        var subFilePath = urlPath;
        if (ResourceType.MainFrame == resourceType || string.IsNullOrWhiteSpace(urlPath))
        {
            subFilePath = Path.Combine(urlPath, DEFAULT_INDEX_FILE);
        }

        var hostCachePath = Path.Combine(CachePath, uri.Host);
        var fullFilePath = Path.Combine(hostCachePath, subFilePath);
        return fullFilePath;
    }
}