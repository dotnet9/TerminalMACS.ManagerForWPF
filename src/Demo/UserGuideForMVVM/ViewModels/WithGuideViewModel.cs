using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using UserGuideForMVVM.Controls;

namespace UserGuideForMVVM.ViewModels;

public class WithGuideViewModel : BindableBase
{
    private ICommand? _showGuideCommand;

    public ICommand ShowGuideCommand
    {
        get { return _showGuideCommand ??= new DelegateCommand<List<object>>(ShowGuideBox); }
    }

    private GuideInfo? _fileGuide;

    public GuideInfo FileGuide
    {
        get { return _fileGuide ??= new GuideInfo { Title = "文件菜单", Content = "新建、打开文件都在这里", ButtonContent = "我知道了" }; }
    }

    private GuideInfo? _editGuide;

    public GuideInfo EditGuide
    {
        get
        {
            return _editGuide ??= new GuideInfo { Title = "编辑菜单", Content = "复制、切换、粘贴等操作在这里哦", ButtonContent = "我知道了" };
        }
    }

    private GuideInfo? _loginGuide;

    public GuideInfo LoginGuide
    {
        get
        {
            return _loginGuide ??= new GuideInfo
                { Title = "登录后惊喜", Content = "登录后，能保存下载记录、多平台共享", ButtonContent = "我知道了" };
        }
    }

    private GuideInfo? _beautyGuide;

    public GuideInfo BeautyGuide
    {
        get
        {
            return _beautyGuide ??= new GuideInfo
            {
                Title = "泰勒·斯威夫特", Content = "泰勒·斯威夫特（Taylor Swift），1989年12月13日出生于美国宾夕法尼亚州，美国女歌手、词曲作者、音乐制作人、演员。",
                ButtonContent = "我知道了"
            };
        }
    }

    private GuideInfo? _logoutGuide;

    public GuideInfo LogoutGuide
    {
        get { return _logoutGuide ??= new GuideInfo { Title = "注销", Content = "切换账号登录？", ButtonContent = "我知道了" }; }
    }

    private GuideInfo? _showGuide;

    public GuideInfo ShowGuide
    {
        get { return _showGuide ??= new GuideInfo { Title = "显示引导", Content = "点击这里再次出现引导哦", ButtonContent = "关闭" }; }
    }

    private static void ShowGuideBox(List<object> guideList)
    {
        var list = new List<GuideInfo>();
        foreach (var obj in guideList.OfType<FrameworkElement>())
        {
            var guide = (GuideInfo)obj.Tag!;
            guide.Uc = obj;
            list.Add(guide);
        }

        var win = new GuideWindow(Window.GetWindow(list[0].Uc)!, list);

        win.ShowDialog();
    }
}