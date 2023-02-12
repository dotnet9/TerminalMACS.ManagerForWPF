using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Shared.Utils
{
    public static class CommonWindow
    {
        public static bool? ShowDialog(Window window, bool bOwner = true)
        {
            if (bOwner)
            {
                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            return window.ShowDialog();
        }

        public static void Show(Window window, bool bOwner = true)
        {
            if (bOwner)
            {
                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            window.Show();
        }

        public static bool IsMinimized
        {
            get
            {
                return Application.Current.MainWindow.WindowState == System.Windows.WindowState.Minimized;
            }
        }

        public static bool IsMaximized
        {
            get
            {
                return Application.Current.MainWindow.WindowState == System.Windows.WindowState.Maximized;
            }
        }

        public static bool IsNormal
        {
            get
            {
                return Application.Current.MainWindow.WindowState == System.Windows.WindowState.Normal;
            }
        }

        public static void ShowMainWindowMinimized()
        {
            Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        public static void ShowMainWindowNormal()
        {
            Application.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            Application.Current.MainWindow.Activate();
        }

        public static void ActivateMainWindow()
        {
            Application.Current.MainWindow.Activate();
        }


    }
}
