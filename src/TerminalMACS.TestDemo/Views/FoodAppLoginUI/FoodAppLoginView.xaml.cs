using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPFXmlTranslator;

namespace TerminalMACS.TestDemo.Views.FoodAppLoginUI;

/// <summary>
///     FoodAppLoginView.xaml 的交互逻辑
/// </summary>
public partial class FoodAppLoginView : Window, INotifyPropertyChanged
{
    private bool isJustStarted = true;

    private bool isLoggedIn;
    private string loginStatusMsg = "";

    public FoodAppLoginView()
    {
        InitializeComponent();
    }

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

    public string LoginStatusmsg
    {
        get => loginStatusMsg;
        set
        {
            if (value != loginStatusMsg)
            {
                loginStatusMsg = value;
                NotifyPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private async Task<bool> ValidateCreds()
    {
        // 模拟登录
        // 你可以发送登录信息到服务器，得到认证回馈
        await Task.Delay(TimeSpan.FromSeconds(2));
        var gen = new Random(DateTime.Now.Millisecond);
        var loginProb = gen.Next(100);
        return loginProb <= 20;
    }

    private async void OpenCB_DialogOpened(object sender, DialogOpenedEventArgs eventArgs)
    {
        try
        {
            IsJustStarted = true;
            LoginStatusmsg = "";
            var isLoggedIn = await ValidateCreds();
            if (isLoggedIn)
                // 需要关闭登录对话框并显示主窗口
                eventArgs.Session.Close(true);
            else
                // 登录失败，设置false作为参数
                eventArgs.Session.Close(false);
        }
        catch (Exception)
        {
            //throw;
        }
    }

    private void ClosingCB_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
    {
        if (eventArgs.Parameter == null) return;
        IsLoggedIn = (bool)eventArgs.Parameter;
        IsJustStarted = false;
        if (IsLoggedIn)
            LoginStatusmsg = I18nManager.Instance.GetResource(Localization.FoodAppLoginView.Success);
        else
            LoginStatusmsg = I18nManager.Instance.GetResource(Localization.FoodAppLoginView.Fail);
    }
}