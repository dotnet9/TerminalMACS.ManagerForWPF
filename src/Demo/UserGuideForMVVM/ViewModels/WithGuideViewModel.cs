using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using UserGuideForMVVM.Controls;

namespace UserGuideForMVVM.ViewModels;

public class WithGuideViewModel : BindableBase
{
    private GuideInfo? _beautyImageGuide;

    private GuideInfo? _bottomCenterBtnuide;

    private GuideInfo? _editMenuItemGuide;

    private GuideInfo? _fileMenuItemGuide;

    private GuideInfo? _lefBottomBtnGuide;

    private GuideInfo? _lefCenterBtnGuide;

    private GuideInfo? _lefTopBtnGuide;

    private GuideInfo? _rightBottomBtnGuide;

    private GuideInfo? _rightCenterBtnGuide;
    private ICommand? _showGuideCommand;

    private ICommand? _showOneGuideCommand;

    private GuideInfo? _topCenterBtnGuide;

    private GuideInfo? _topRightBtnGuide;

    public ICommand ShowGuideCommand
    {
        get { return _showGuideCommand ??= new DelegateCommand<List<object>>(GuideWindow.ShowGuideBox); }
    }

    public ICommand ShowOneGuideCommand
    {
        get
        {
            return _showOneGuideCommand ??=
                new DelegateCommand<object>(x => GuideWindow.ShowGuideBox(new List<object> { x }));
        }
    }

    public GuideInfo FileMenuItemGuide
    {
        get
        {
            return _fileMenuItemGuide ??= new GuideInfo
                { Title = "文件菜单引导", Content = "创建文件、打开文件等操作点击这里", ButtonContent = "我知道了" };
        }
    }

    public GuideInfo EditMenuItemGuide
    {
        get
        {
            return _editMenuItemGuide ??= new GuideInfo
                { Title = "编辑菜单引导", Content = "复制、切换、粘贴等操作在这里哦", ButtonContent = "我知道了" };
        }
    }

    public GuideInfo LefTopBtnGuide
    {
        get
        {
            return _lefTopBtnGuide ??= new GuideInfo
                { Title = "控件在窗体左上角时", Content = "引导框左上角显示在该控件左下角", ButtonContent = "我知道了" };
        }
    }

    public GuideInfo TopCenterBtnGuide
    {
        get
        {
            return _topCenterBtnGuide ??= new GuideInfo
            {
                Title = "控件在窗体上中时", Content = "引导框左上角显示在该控件左下角",
                ButtonContent = "我知道了"
            };
        }
    }

    public GuideInfo TopRightBtnGuide
    {
        get
        {
            return _topRightBtnGuide ??= new GuideInfo
            {
                Title = "控件在窗体右上时",
                Content = "引导框右上角显示在该控件右下角",
                ButtonContent = "我知道了"
            };
        }
    }

    public GuideInfo LefCenterBtnGuide
    {
        get
        {
            return _lefCenterBtnGuide ??= new GuideInfo
            {
                Title = "控件在窗体左侧中间时",
                Content = "引导框左上角显示在该控件左下角",
                ButtonContent = "我知道了"
            };
        }
    }

    public GuideInfo BeautyImageGuide
    {
        get
        {
            return _beautyImageGuide ??= new GuideInfo
            {
                Title = "美女在这",
                Content = "点击这里显示引导",
                ButtonContent = "我点点点"
            };
        }
    }

    public GuideInfo RightCenterBtnGuide
    {
        get
        {
            return _rightCenterBtnGuide ??= new GuideInfo
            {
                Title = "控件在窗体右侧中间时",
                Content = "引导框右上角显示在该控件右下角",
                ButtonContent = "我知道了"
            };
        }
    }

    public GuideInfo LefBottomBtnGuide
    {
        get
        {
            return _lefBottomBtnGuide ??= new GuideInfo
            {
                Title = "控件在窗体左下角时",
                Content = "引导框左下角显示在该控件左上角",
                ButtonContent = "我知道了"
            };
        }
    }

    public GuideInfo BottomCenterBtnuide
    {
        get
        {
            return _bottomCenterBtnuide ??= new GuideInfo
            {
                Title = "控件在窗体下侧中间时",
                Content = "引导框左下角显示在该控件左上角",
                ButtonContent = "我知道了"
            };
        }
    }

    public GuideInfo RightBottomBtnGuide
    {
        get
        {
            return _rightBottomBtnGuide ??= new GuideInfo
            {
                Title = "控件在窗体右下角时",
                Content = "引导框右下角显示在该控件左上角",
                ButtonContent = "我知道了"
            };
        }
    }
}