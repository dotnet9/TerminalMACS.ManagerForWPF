using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfExtensions.Xaml;

namespace TerminalMACS.TestDemo.Views.FoodAppLoginUI
{
    /// <summary>
    /// FoodAppLoginView.xaml 的交互逻辑
    /// </summary>
    public partial class FoodAppLoginView : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set
            {
                if (value != isLoggedIn)
                {
                    isLoggedIn = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool isJustStarted = true;

        public bool IsJustStarted
        {
            get => isJustStarted;
            set
            {
                if (value != isJustStarted)
                {
                    isJustStarted = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string loginStatusMsg = "";
        public string LoginStatusmsg
        {
            get => loginStatusMsg;
            set
            {
                if(value!=loginStatusMsg)
                {
                    loginStatusMsg = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public FoodAppLoginView()
        {
            InitializeComponent();
        }

        private void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private async Task<bool> ValidateCreds()
        {
            // 模拟登录
            // 你可以发送登录信息到服务器，得到认证回馈
            await Task.Delay(TimeSpan.FromSeconds(2));
            Random gen = new Random(DateTime.Now.Millisecond);
            int loginProb = gen.Next(100);
            return loginProb <= 20;
        }

        private async void OpenCB_DialogOpened(object sender, MaterialDesignThemes.Wpf.DialogOpenedEventArgs eventArgs)
        {
            try
            {
                this.IsJustStarted = true;
                this.LoginStatusmsg = "";
                bool isLoggedIn = await ValidateCreds();
                if (isLoggedIn)
                {
                    // 需要关闭登录对话框并显示主窗口
                    eventArgs.Session.Close(true);
                }
                else
                {
                    // 登录失败，设置false作为参数
                    eventArgs.Session.Close(false);
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void ClosingCB_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter == null)
            {
                return;
            }
            IsLoggedIn = (bool)eventArgs.Parameter;
            IsJustStarted = false;
            if(IsLoggedIn)
            {
                this.LoginStatusmsg = I18nManager.Instance.Get(I18nResources.Language.FoodAppLoginView_Success).ToString();
            }
            else
            {
                this.LoginStatusmsg = I18nManager.Instance.Get(I18nResources.Language.FoodAppLoginView_Fail).ToString();
            }
        }
    }
}
