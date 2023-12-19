using SocketCore.Logs.Models;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SocketCore.Logs;

public partial class LogView : UserControl
{
    private const int MaxCount = 1000;
    private static InlineCollection? _inlines;

    private static readonly Dictionary<LogType, Brush> LogTypeBrushes = new()
    {
        { LogType.Debug, Brushes.LightSeaGreen },
        { LogType.Info, Brushes.Green },
        { LogType.Warning, Brushes.DarkOrange },
        { LogType.Error, Brushes.OrangeRed }
    };

    public LogView()
    {
        InitializeComponent();

        var graph = new Paragraph();
        _inlines = graph.Inlines;
        LogRichTextBox.Document.Blocks.Clear();
        LogRichTextBox.Document.Blocks.Add(graph);

        ReadLog();
    }

    private void ReadLog()
    {
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    if (Logger.Logs.TryTake(out var log, TimeSpan.FromMilliseconds(10)))
                    {
                        LogRichTextBox.Dispatcher.BeginInvoke(() =>
                        {
                            LogRichTextBox.BeginChange();

                            _inlines?.Add(new Run($"{log.Time:yyyy-MM-dd HH:mm:ss fff} {log.Content}\r\n")
                                { Foreground = LogTypeBrushes[log.Type] });
                            if (_inlines?.Count > MaxCount)
                            {
                                _inlines.Remove(_inlines.FirstInline);
                            }

                            LogRichTextBox.ScrollToEnd();
                            LogRichTextBox.EndChange();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"日志读取失败，糟了：{ex.Message}");
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }
        });
    }
}