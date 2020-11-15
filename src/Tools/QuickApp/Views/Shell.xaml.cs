using IWshRuntimeLibrary;
using Microsoft.Win32;
using QuickApp.Helpers;
using QuickApp.Models;
using QuickApp.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace QuickApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {

        public ShellViewModel ViewModel
        {
            get { return this.DataContext as ShellViewModel; }
            set { this.DataContext = value; }
        }

        public Shell()
        {
            InitializeComponent();

            if (this.ViewModel == null)
                this.ViewModel = new ShellViewModel();
            LoadAllMenus();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.Loaded += MainWindow_Loaded;
            this.Deactivated += MainWindow_Deactivated;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            Shell window = (Shell)sender;
            window.Topmost = true;
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Top = desktopWorkingArea.Top;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btnOpenOrClose_Click(object sender, RoutedEventArgs e)
        {
            if (btnOpenOrClose.IsChecked == true)
            {
                DoubleAnimation StopAnimation = new DoubleAnimation();
                StopAnimation.From = 0;
                StopAnimation.To = -80;
                StopAnimation.Duration = new Duration(TimeSpan.Parse("0:0:0.5"));
                _this.BeginAnimation(Shell.TopProperty, StopAnimation);
                btnOpenOrClose.ToolTip = "展开";
            }
            else
            {
                DoubleAnimation OpenAnimation = new DoubleAnimation();
                OpenAnimation.From = -80;
                OpenAnimation.To = 0.1;
                OpenAnimation.Duration = new Duration(TimeSpan.Parse("0:0:0.5"));
                _this.BeginAnimation(Shell.TopProperty, OpenAnimation);
                btnOpenOrClose.ToolTip = "收起";
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                MenuItemInfo menuItem = new MenuItemInfo() { FilePath = fileName };

                // 快捷方式需要获取目标文件路径
                if (fileName.ToLower().EndsWith("lnk"))
                {
                    WshShell shell = new WshShell();
                    IWshShortcut wshShortcut = (IWshShortcut)shell.CreateShortcut(fileName);
                    menuItem.FilePath = wshShortcut.TargetPath;
                }
                ImageSource imageSource = SystemIcon.GetImageSource(true, menuItem.FilePath);
                System.IO.FileInfo file = new System.IO.FileInfo(fileName);
                if (string.IsNullOrWhiteSpace(file.Extension))
                {
                    menuItem.Name = file.Name;
                }
                else
                {
                    menuItem.Name = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
                }
                menuItem.Type = MenuItemType.Exe;

                if (ConfigHelper.AddNewMenuItem(menuItem))
                {
                    var btn = AddMenuItem(menuItem);
                    fishButtons.Children.Add(btn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        void LoadAllMenus()
        {
            this.fishButtons.Children.Clear();
            foreach (var item in ConfigHelper.GetAllMenuItems())
            {
                var btn = AddMenuItem(item);
                fishButtons.Children.Add(btn);
            }
        }

        Button AddMenuItem(MenuItemInfo menuItem)
        {
            ImageSource imageSource = null;
            if (!string.IsNullOrWhiteSpace(menuItem.IconPath))
            {
                imageSource = SystemIcon.GetImageSource($"{AppDomain.CurrentDomain.BaseDirectory}Images/{menuItem.IconPath}");
            }
            else if (System.IO.File.Exists(menuItem.FilePath))
            {
                imageSource = SystemIcon.GetImageSource(true, menuItem.FilePath);
            }
            var btn = new Button()
            {
                ToolTip = menuItem.Name,
                Content = new Image() { Source = imageSource }
            };
            btn.Click += (s, e) => RunExe(menuItem);

            return btn;
        }
        Process processLong = new Process();
        void RunExe(MenuItemInfo menuItem)
        {
            try
            {
                if (menuItem.Type == MenuItemType.Exe || menuItem.Type==MenuItemType.Cmd)
                {
                    Process.Start(menuItem.FilePath);
                }
                else if (menuItem.Type == MenuItemType.Web)
                {
                    Process.Start("explorer.exe", menuItem.FilePath);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}