namespace DotNetBrowserDemo;

public partial class MainWindow : Window
{
    private readonly IEngine engine;

    public MainWindow()
    {
        engine = EngineFactory.Create();

        var browser = engine.CreateBrowser();
        browser.Navigation.LoadUrl("https://dotnet9.com");

        InitializeComponent();

        this.MyBrowserView.InitializeFrom(browser);
        Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object? sender, System.EventArgs e)
    {
        engine.Dispose();
    }
}