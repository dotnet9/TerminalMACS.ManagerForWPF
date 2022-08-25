using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfAppForZoomInAndZoomOut.Models;
using WpfAppForZoomInAndZoomOut.ViewModels;

namespace WpfAppForZoomInAndZoomOut;

public partial class ChildView1 : UserControl
{
    public ChildView1()
    {
        ViewModel = new ChildView1ViewModel();
        InitializeComponent();
    }

    public ChildView1ViewModel? ViewModel
    {
        get => DataContext as ChildView1ViewModel;
        set => DataContext = value;
    }


    private void NewSelectedItemChanged(object sender, KeyboardFocusChangedEventArgs e)
    {
        //var item = (ListBoxItem)sender;
        //var testModel = (TestModel)item.DataContext;
        //this.ViewModel!.ItemSource.Where(x => !x.Equals(testModel)).ToList().ForEach(x => x.IsSelected = false);
        //testModel.IsSelected = true;
    }

    private void TestListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control) return;

        var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArgs.RoutedEvent = MouseWheelEvent;
        eventArgs.Source = sender;
        ((FrameworkElement)sender).RaiseEvent(eventArgs);
    }

    //private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
    //{
    //    var textBox = (TextBox)sender;
    //    if (textBox.SelectedText.Length > 0)
    //    {
    //        Popup.PlacementTarget = textBox;
    //        Popup.IsOpen = true;
    //    }
    //    else
    //    {
    //        Popup.IsOpen = false;
    //    }
    //}

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        //Popup.IsOpen = false;
    }

    private void TestListBoxIns_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel!.DragStart(sender, e);
    }

    private void TestListBoxIns_OnDragOver(object sender, DragEventArgs e)
    {
        ViewModel!.DragOver(sender, e);
    }

    private void TestListBoxIns_OnDrop(object sender, DragEventArgs e)
    {
        ViewModel!.Drop(sender, e);
    }

    private void Capture_Click(object sender, RoutedEventArgs e)
    {
        Capture(false);
    }

    public static void OpenFolderAndSelectFile(string fileFullName)
    {
        var psi = new ProcessStartInfo("Explorer.exe");
        psi.Arguments = "/e,/select," + fileFullName;
        Process.Start(psi);
    }

    private void CaptureAll_Click(object sender, RoutedEventArgs e)
    {
        Capture(true);
    }

    private void Capture(bool all)
    {
        var dlg = new SaveFileDialog()
        {
            DefaultExt = ".png",
            Filter = "PNG image (*.png)|*.png|All files (*.*)|*.*"
        };
        var result = dlg.ShowDialog();
        if (result == false) return;

        if (File.Exists(dlg.FileName) && new FileInfo(dlg.FileName).Length != 0)
            File.Delete(dlg.FileName);

        const int offset = 150;
        var captureWidth = TestListBoxIns.ActualWidth - offset * 2;
        var captureHeight = all ? TestListBoxIns.DesiredSize.Height : 500;

        var pngEncoder = new PngBitmapEncoder();
        var renderTargetBitmap = new RenderTargetBitmap((int)TestListBoxIns.ActualWidth,
            (int)TestListBoxIns.ActualHeight, 96d, 96d, PixelFormats.Default);
        renderTargetBitmap.Render(TestListBoxIns);
        var bitmap = new CroppedBitmap(renderTargetBitmap,
            new Int32Rect(offset, 0, (int)captureWidth, (int)captureHeight));
        pngEncoder.Frames.Add(BitmapFrame.Create(bitmap));
        using (var fs = File.OpenWrite(dlg.FileName))
        {
            pngEncoder.Save(fs);
        }

        OpenFolderAndSelectFile(dlg.FileName);
    }
}