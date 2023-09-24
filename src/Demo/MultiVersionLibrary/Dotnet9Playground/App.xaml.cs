using Dotnet9HookHigh;
using Dotnet9Playground.Hooks;
using System.Windows;

namespace Dotnet9Playground
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 拦截气球动画播放方法
            HookBallGameStartGame.StartHook();

            // 这是第二个拦截方法：拦截气球MeasureOverride方法
            HookBallgameMeasureOverride.StartHook();
        }
    }
}