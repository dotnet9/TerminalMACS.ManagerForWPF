using CefSharp;
using System.IO;

namespace WpfWithCefSharpCacheDemo.Caches;

internal class CefResponseFilter : IResponseFilter
{
    public string LocalCacheFilePath { get; set; }
    private const int BUFFER_LENGTH = 1024;
    private bool isFailCacheFile;


    public FilterStatus Filter(Stream? dataIn, out long dataInRead, Stream? dataOut, out long dataOutWritten)
    {
        dataInRead = 0;
        dataOutWritten = 0;

        if (dataIn == null)
        {
            return FilterStatus.NeedMoreData;
        }

        var length = dataIn.Length;
        var data = new byte[BUFFER_LENGTH];
        var count = dataIn.Read(data, 0, BUFFER_LENGTH);

        dataInRead = count;
        dataOutWritten = count;

        dataOut?.Write(data, 0, count);

        try
        {
            CacheFile(data, count);
        }
        catch
        {
            // ignored
        }

        return length == dataIn.Position ? FilterStatus.Done : FilterStatus.NeedMoreData;
    }

    public bool InitFilter()
    {
        try
        {
            var dirPath = Path.GetDirectoryName(LocalCacheFilePath);
            if (!string.IsNullOrWhiteSpace(dirPath) && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
        catch
        {
            // ignored
        }

        return true;
    }

    public void Dispose()
    {
    }

    private void CacheFile(byte[] data, int count)
    {
        if (isFailCacheFile)
        {
            return;
        }

        try
        {
            if (!File.Exists(LocalCacheFilePath))
            {
                using var fs = File.Create(LocalCacheFilePath);
                fs.Write(data, 0, count);
            }
            else
            {
                using var fs = File.Open(LocalCacheFilePath, FileMode.Append);
                fs.Write(data,0,count);
            }
        }
        catch
        {
            isFailCacheFile = true;
            File.Delete(LocalCacheFilePath);
        }
    }
}