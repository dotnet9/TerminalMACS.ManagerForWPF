using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace RPA.Shared.UI
{
    /// <summary>
    /// Windows窗口扩展类，目前主要对图标进行显示隐藏控制
    /// </summary>
    public class WindowEx
    {
        private const int GwlExstyle = -20;
        private const int SwpFramechanged = 0x0020;
        private const int SwpNomove = 0x0002;
        private const int SwpNosize = 0x0001;
        private const int SwpNozorder = 0x0004;
        private const int WsExDlgmodalframe = 0x0001;

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint msg,
          IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
          int x, int y, int width, int height, uint flags);


        /// <summary>
        /// 是否隐藏窗体图标
        /// </summary>
        public static readonly DependencyProperty ShowIconProperty =
          DependencyProperty.RegisterAttached(
            "ShowIcon",
            typeof(bool),
            typeof(WindowEx),
            new FrameworkPropertyMetadata(true, new PropertyChangedCallback((d, e) => RemoveIcon((Window)d))));


        public static Boolean GetShowIcon(UIElement element)
        {
            return (Boolean)element.GetValue(ShowIconProperty);
        }

        public static void RemoveIcon(Window window)
        {
            //不显示左上角的应用图标
            window.SourceInitialized += delegate
            {
                var hwnd = new WindowInteropHelper(window).Handle;

                int extendedStyle = GetWindowLong(hwnd, GwlExstyle);
                SetWindowLong(hwnd, GwlExstyle, extendedStyle | WsExDlgmodalframe);

                SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SwpNomove |
                  SwpNosize | SwpNozorder | SwpFramechanged);
            };
        }

        public static void SetShowIcon(UIElement element, Boolean value)
        {
            element.SetValue(ShowIconProperty, value);
        }




        public static bool GetShowMinimizeBox(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowMinimizeBoxProperty);
        }

        public static void SetShowMinimizeBox(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowMinimizeBoxProperty, value);
        }

        /// <summary>
        /// 是否显示最小化按钮
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeBoxProperty =
            DependencyProperty.RegisterAttached("ShowMinimizeBox", typeof(bool), typeof(WindowEx)
                , new FrameworkPropertyMetadata(true, new PropertyChangedCallback((d, e) => RemoveMinimizeBox((Window)d))));

        private static void RemoveMinimizeBox(Window window)
        {
            //禁用最小化按钮
            window.SourceInitialized += delegate
            {
                var hwnd = new WindowInteropHelper(window).Handle;
                var value = GetWindowLong(hwnd, GWL_STYLE);
                SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MINIMIZEBOX));
            };
        }

        public static bool GetShowMaximizeBox(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowMaximizeBoxProperty);
        }

        public static void SetShowMaximizeBox(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowMaximizeBoxProperty, value);
        }

        /// <summary>
        /// 是否显示最大化按钮
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeBoxProperty =
            DependencyProperty.RegisterAttached("ShowMaximizeBox", typeof(bool), typeof(WindowEx)
                , new FrameworkPropertyMetadata(true, new PropertyChangedCallback((d, e) => RemoveMaximizeBox((Window)d))));

        private static void RemoveMaximizeBox(Window window)
        {
            //禁用最大化按钮
            window.SourceInitialized += delegate
            {
                var hwnd = new WindowInteropHelper(window).Handle;
                var value = GetWindowLong(hwnd, GWL_STYLE);
                SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MAXIMIZEBOX));
            };
        }
    }
}
