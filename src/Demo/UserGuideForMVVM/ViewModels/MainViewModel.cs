using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using UserGuideForMVVM.Controls;

namespace UserGuideForMVVM.ViewModels;

public class MainViewModel : BindableBase
{
    private ICommand? _showGuideCommand;

    public ICommand ShowGuideCommand
    {
        get { return _showGuideCommand ??= new DelegateCommand<List<object>>(ShowGuideBox); }
    }

    private GuideInfo? _showNoBorderWindowGuide;

    public GuideInfo ShowNoBorderWindowGuide
    {
        get
        {
            return _showNoBorderWindowGuide ??= new GuideInfo
                { Title = "窗体没边框的情况？", Content = "引导需要做相应的偏移操作", ButtonContent = "我知道了" };
        }
    }

    private GuideInfo? _showNormalWindowGuide;

    public GuideInfo ShowNormalWindowGuide
    {
        get
        {
            return _showNormalWindowGuide ??= new GuideInfo
                { Title = "无边框窗体的引导？", Content = "默认不需要偏移", ButtonContent = "我知道了" };
        }
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