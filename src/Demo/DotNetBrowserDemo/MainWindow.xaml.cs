using System;

namespace DotNetBrowserDemo;

public partial class MainWindow : Window
{
    private IEngine? engine;
    private IBrowser? browser;
    private string? currentUrl="www.baidu.com";

    public MainWindow()
    {
        CreateEngine(DotNetBrowser.Ui.Language.Chinese);
    }

    private void CreateEngine(DotNetBrowser.Ui.Language language)
    {
        DisposeEngine();
        engine = EngineFactory.Create(new EngineOptions.Builder
        {
            Language = language
        }.Build());

        browser = engine.CreateBrowser();
        browser.Navigation.LoadUrl(currentUrl);

        InitializeComponent();
        
        this.MyBrowserView.InitializeFrom(browser);
    }

    private void DisposeEngine()
    {
        if (engine == null)
        {
            return;
        }
        engine.Dispose();
        engine = null;
    }

    private void MainWindow_Closed(object? sender, System.EventArgs e)
    {
        engine?.Dispose();
    }

    private void ChangeRightUrl_Click(object sender, RoutedEventArgs e)
    {
        string[] rightUrls = { "www.baidu.com", "www.google.com", "https://dotnet9.com" };
        currentUrl = rightUrls[Random.Shared.Next(rightUrls.Length)];
        browser.Navigation.LoadUrl(currentUrl);
    }

    private void ChangeFalseUrl_Click(object sender, RoutedEventArgs e)
    {
        currentUrl = $"www.{Random.Shared.Next(100000)}.com";
        browser.Navigation.LoadUrl(currentUrl);
    }

    private void ChangeToChinese_Click(object sender, RoutedEventArgs e)
    {
        CreateEngine(DotNetBrowser.Ui.Language.Chinese);
    }

    private void ChangeToEnglish_Click(object sender, RoutedEventArgs e)
    {
        CreateEngine(DotNetBrowser.Ui.Language.EnglishUs);
    }
}