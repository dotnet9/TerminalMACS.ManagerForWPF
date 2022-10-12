using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Handlers;
using DotNetBrowser.Input.Keyboard;
using DotNetBrowser.Input.Keyboard.Events;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;
using DotNetBrowser.Search.Handlers;
using DotNetBrowser.Ui;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.MessageBox;

namespace DotNetBrowserDemo;

public partial class MainWindow : Window
{
    private IBrowser? _browser;
    private string? _currentUrl = "www.baidu.com";
    private IEngine? _engine;

    public MainWindow()
    {
        InitializeComponent();
        CreateEngine(DotNetBrowser.Ui.Language.Chinese);
    }

    private void CreateEngine(Language language, bool useRemoteDebuggingPort = false)
    {
        DisposeEngine();

        EngineOptions CreateOption()
        {
            if (useRemoteDebuggingPort)
                return new EngineOptions.Builder
                {
                    RenderingMode = RenderingMode.HardwareAccelerated,
                    RemoteDebuggingPort = 9222,
                    Language = language
                }.Build();
            return new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated,
                Language = language
            }.Build();
        }

        _engine = EngineFactory.Create(CreateOption());

        _browser = _engine.CreateBrowser();
        _browser.Navigation.LoadUrl(_currentUrl);

        MyBrowserView.InitializeFrom(_browser);
    }

    private void DisposeEngine()
    {
        if (_engine == null) return;

        _engine.Dispose();
        _engine = null;
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
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
                var options = new UrlRequestJobOptions
                {
                    Headers = new List<HttpHeader>
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

        var engineOptions = new EngineOptions.Builder
        {
            Schemes = { { Scheme.Create("https"), handler } }
        }.Build();

        _engine = EngineFactory.Create(engineOptions);
        _browser = _engine.CreateBrowser();
        MyBrowserView.InitializeFrom(_browser);
        var loadResult = _browser.Navigation.LoadUrl("https://www.baidu.com").Result;
        var str = new StringBuilder($"Load result: {loadResult}");
        str.AppendLine($"HTML: {_browser.MainFrame.Html}");
        Debug.WriteLine(str.ToString());
    }

    private void OpenRemoteDebuggingPort_Click(object sender, RoutedEventArgs e)
    {
        CreateEngine(DotNetBrowser.Ui.Language.EnglishUs, true);
        var debugUrl = _browser!.DevTools.RemoteDebuggingUrl;
        Clipboard.SetText(debugUrl);
        MessageBox.Show($"调试地址已经复制到剪贴板：{debugUrl}");
        _browser.DevTools.Show();
    }

    private void SimulateKey_Click(object sender, RoutedEventArgs e)
    {
        var keyboard = _browser!.Keyboard;
        SimulateKey(keyboard, KeyCode.VkH, "H");
        SimulateKey(keyboard, KeyCode.VkE, "e");
        SimulateKey(keyboard, KeyCode.VkL, "l");
        SimulateKey(keyboard, KeyCode.VkL, "l");
        SimulateKey(keyboard, KeyCode.VkO, "o");
        SimulateKey(keyboard, KeyCode.Space, " ");
        // Simulate input of some non-letter characters.
        SimulateKey(keyboard, KeyCode.Vk5, "%", new KeyModifiers { ShiftDown = true });
        SimulateKey(keyboard, KeyCode.Vk2, "@", new KeyModifiers { ShiftDown = true });
    }

    private static void SimulateKey(IKeyboard keyboard, KeyCode key, string keyChar,
        KeyModifiers? modifiers = null)
    {
        modifiers ??= new KeyModifiers();
        var keyDownEventArgs = new KeyPressedEventArgs
        {
            KeyChar = keyChar,
            VirtualKey = key,
            Modifiers = modifiers
        };

        var keyPressEventArgs = new KeyTypedEventArgs
        {
            KeyChar = keyChar,
            VirtualKey = key,
            Modifiers = modifiers
        };
        var keyUpEventArgs = new KeyReleasedEventArgs
        {
            VirtualKey = key,
            Modifiers = modifiers
        };

        keyboard.KeyPressed.Raise(keyDownEventArgs);
        keyboard.KeyTyped.Raise(keyPressEventArgs);
        keyboard.KeyReleased.Raise(keyUpEventArgs);
    }

    private void SearchWords_Click(object sender, RoutedEventArgs e)
    {
        var searchText = TxtSearchWords.Text;

        IHandler<FindResultReceivedParameters> intermediateResultsHandler =
            new Handler<FindResultReceivedParameters>(ProcessSearchResults);

        Console.WriteLine("Find text (1/2)");
        var textFinder = _browser.TextFinder;
        var findResult =
            textFinder.Find(searchText, null, intermediateResultsHandler)
                .Result;

        var selectedMatch = findResult.SelectedMatch;
        var count = findResult.NumberOfMatches;
        Console.WriteLine($"Find Result: {selectedMatch}/{count}");

        Console.WriteLine("Find text (2/2)");
        findResult = textFinder
            .Find(searchText, null, intermediateResultsHandler)
            .Result;

        selectedMatch = findResult.SelectedMatch;
        count = findResult.NumberOfMatches;
        Console.WriteLine($"Find Result: {selectedMatch}/{count}");

        textFinder.StopFinding();
    }

    private static void ProcessSearchResults(FindResultReceivedParameters args)
    {
        var result = args.FindResult;

        if (args.IsSearchFinished)
            Console.WriteLine("Found: "
                              + result.SelectedMatch
                              + "/"
                              + result.NumberOfMatches);
        else
            Console.WriteLine("Search in progress... Found "
                              + result.SelectedMatch
                              + "/"
                              + result.NumberOfMatches);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new FolderBrowserDialog();
        var result = dialog.ShowDialog();

        if (result == System.Windows.Forms.DialogResult.Cancel)
        {
            return;
        }

        var saveFolder = dialog.SelectedPath.Trim();
        var filePath = Path.GetFullPath($"{saveFolder}\\index.html");
        var dirPath = Path.GetFullPath($"{saveFolder}\\resources");
        Directory.CreateDirectory(dirPath);

        if (_browser?.SaveWebPage(filePath, dirPath, SavePageType.CompletePage) == true)
        {
            Console.WriteLine("The web page has been saved to " + filePath);
        }
        else
        {
            Console.WriteLine("Failed to save the web page to " + filePath);
        }

        OpenFolderAndSelectFile(saveFolder);
    }

    public static void OpenFolderAndSelectFile(string fileFullName)
    {
        var psi = new ProcessStartInfo("Explorer.exe")
        {
            Arguments = "/e,/select," + fileFullName
        };
        Process.Start(psi);
    }
}