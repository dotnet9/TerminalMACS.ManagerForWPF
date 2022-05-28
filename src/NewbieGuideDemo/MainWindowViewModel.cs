using Dotnet9WPFControls.Controls;
using Prism.Mvvm;
using System.Collections.Generic;

namespace NewbieGuideDemo
{
    public class MainWindowViewModel : BindableBase
    {
        private GuideInfo? _guide;

        public GuideInfo Guide =>
            _guide ??= new GuideInfo("快速添加新手引导", "这样添加新手引导，或许比较优雅");

        public List<GuideInfo> Guides => new() {Guide};
    }
}