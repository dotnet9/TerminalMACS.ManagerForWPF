using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace DotNetBrowserDemo;

public partial class MainWindow : Window
{
    private IEngine? _engine;
    private IBrowser? _browser;
    private string? _currentUrl = "www.baidu.com";

    public MainWindow()
    {
        InitializeComponent();
        CreateEngine(DotNetBrowser.Ui.Language.Chinese);
    }

    private void CreateEngine(DotNetBrowser.Ui.Language language)
    {
        DisposeEngine();
        _engine = EngineFactory.Create(new EngineOptions.Builder
        {
            Language = language
        }.Build());

        _browser = _engine.CreateBrowser();
        _browser.Navigation.LoadUrl(_currentUrl);

        this.MyBrowserView.InitializeFrom(_browser);
    }

    private void DisposeEngine()
    {
        if (_engine == null)
        {
            return;
        }

        _engine.Dispose();
        _engine = null;
    }

    private void MainWindow_Closed(object? sender, System.EventArgs e)
    {
        _engine?.Dispose();
    }

    private void ChangeRightUrl_Click(object sender, RoutedEventArgs e)
    {
        string[] rightUrls = { "www.baidu.com", "www.google.com", "https://dotnet9.com" };
        _currentUrl = rightUrls[Random.Shared.Next(rightUrls.Length)];
        _browser?.Navigation.LoadUrl(_currentUrl);
    }

    private void ChangeFalseUrl_Click(object sender, RoutedEventArgs e)
    {
        _currentUrl = $"www.{Random.Shared.Next(100000)}.com";
        _browser?.Navigation.LoadUrl(_currentUrl);
    }

    private void ChangeToChinese_Click(object sender, RoutedEventArgs e)
    {
        CreateEngine(DotNetBrowser.Ui.Language.Chinese);
    }

    private void ChangeToEnglish_Click(object sender, RoutedEventArgs e)
    {
        CreateEngine(DotNetBrowser.Ui.Language.EnglishUs);
    }

    private void InterceptTest_Click(object sender, RoutedEventArgs e)
    {
        DisposeEngine();
        var handler = new Handler<InterceptRequestParameters, InterceptRequestResponse>(
            p =>
            {
                var options = new UrlRequestJobOptions()
                {
                    Headers = new List<HttpHeader>()
                    {
                        new("Content-Type", "text/html", "charset=utf-8")
                    }
                };

                var job = p.Network.CreateUrlRequestJob(p.UrlRequest, options);
                Task.Run(() =>
                {
                    job.Write(Encoding.UTF8.GetBytes("Hello world!"));
                    job.Complete();
                });
                return InterceptRequestResponse.Intercept(job);
            });

        var engineOptions = new EngineOptions.Builder()
        {
            Schemes = { { Scheme.Create("https"), handler } }
        }.Build();

        _engine = EngineFactory.Create(engineOptions);
        _browser = _engine.CreateBrowser();
        this.MyBrowserView.InitializeFrom(_browser);
        var loadResult = _browser.Navigation.LoadUrl("https://www.baidu.com").Result;
        var str = new StringBuilder($"Load result: {loadResult}");
        str.AppendLine($"HTML: {_browser.MainFrame.Html}");
        Debug.WriteLine(str.ToString());
    }
}