using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using UserGuideForMVVM.Controls;

namespace UserGuideForMVVM.ViewModels;

public class WithGuidViewModel : BindableBase
{
    private ICommand? _showGuideCommand;

    public ICommand ShowGuideCommand
    {
        get { return _showGuideCommand ??= new DelegateCommand<List<object>>(ShowGuideBox); }
    }

    private static void ShowGuideBox(List<object> guideList)
    {
        var list = new List<GuidVo>();
        foreach (var obj in guideList.OfType<FrameworkElement>())
        {
            var item = new GuidVo
            {
                Uc = obj,
                Content = obj.Tag?.ToString()
            };
            list.Add(item);
        }

        var win = new GuideWindow(Window.GetWindow(list[0].Uc), list);

        win.ShowDialog();
    }
}