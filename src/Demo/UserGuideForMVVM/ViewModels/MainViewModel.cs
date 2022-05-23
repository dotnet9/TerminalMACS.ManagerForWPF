using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using UserGuideForMVVM.Controls;

namespace UserGuideForMVVM.ViewModels;

public class MainViewModel : BindableBase
{
    private ICommand? _showGuideCommand;

    private GuideInfo? _showNoBorderWindowGuide;

    private GuideInfo? _showNormalWindowGuide;

    public ICommand ShowGuideCommand
    {
        get { return _showGuideCommand ??= new DelegateCommand<List<object>>(GuideWindow.ShowGuideBox); }
    }

    public GuideInfo ShowNoBorderWindowGuide
    {
        get
        {
            return _showNoBorderWindowGuide ??= new GuideInfo
                { Title = "窗体没边框的情况", Content = "引导需要做相应的偏移操作", ButtonContent = "我知道了" };
        }
    }

    public GuideInfo ShowNormalWindowGuide
    {
        get
        {
            return _showNormalWindowGuide ??= new GuideInfo
                { Title = "无边框窗体的引导", Content = "默认不需要偏移", ButtonContent = "我知道了" };
        }
    }
}