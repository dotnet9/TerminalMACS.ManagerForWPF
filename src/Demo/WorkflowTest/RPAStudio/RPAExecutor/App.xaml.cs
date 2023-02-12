using Newtonsoft.Json.Linq;
using NLog;
using RPA.Interfaces.Share;
using RPA.Shared.Executor;
using RPA.Shared.Utils;
using RPAExecutor.Executor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RPAExecutor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    //调试参数可设置为:
    //RPAStudio点运行时:B9907AEB-5CD1-4AC4-B995-5CD39C46A889 RPAStudio RPAStudio.exe_B9907AEB-5CD1-4AC4-B995-5CD39C46A889#RUN
    //RPAStudio点调试时:B9907AEB-5CD1-4AC4-B995-5CD39C46A889 RPAStudio RPAStudio.exe_B9907AEB-5CD1-4AC4-B995-5CD39C46A889#DEBUG
    //RPARobot点运行时:B9907AEB-5CD1-4AC4-B995-5CD39C46A889 RPARobot RPARobotServices_TEST@TEST
    //RPACommandLine运行时:B9907AEB-5CD1-4AC4-B995-5CD39C46A889 RPACommandLine RPACommandLine.exe_B9907AEB-5CD1-4AC4-B995-5CD39C46A889

    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private DispatcherTimer _quitTimer { get; set; } = new DispatcherTimer();

        protected string[] Arguments { get; } = Environment.GetCommandLineArgs().Skip(1).ToArray<string>();

        private List<string> _assemblyResolveDllList;

        private RPAExecutorAgent _agent = new RPAExecutorAgent();
        private string _guid;
        private string _rpaFlag;
        private string _controllerIpcName;

        private IWorkflowManager _manager;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if RPA_EXECUTOR_CONSOLE_DEBUG
            Common.OpenConsole();
#endif

            _logger.Debug("程序启动");

            if (Arguments.Length < 3)
            {
                _logger.Debug("参数数量不足，程序退出");
                Environment.Exit(-1);
            }

            _guid = Arguments[0];
            _rpaFlag = Arguments[1];
            _controllerIpcName = Arguments[2];

            _logger.Debug($"启动参数 {_guid} {_rpaFlag} {_controllerIpcName}");

            //异常注册
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            _agent.Init(_rpaFlag, _controllerIpcName);
            _agent.StartEvent += Agent_StartEvent;
            _agent.StopEvent += Agent_StopEvent;
            _agent.ContinueEvent += Agent_ContinueEvent;
            _agent.ExitEvent += Agent_ExitEvent;
            _agent.UpdateAgentConfigEvent += Agent_UpdateAgentConfigEvent;

            _logger.Debug($"agent执行注册指令 guid={_guid},rpaFlag={_rpaFlag}");
            _agent.Register(_guid);
        }

        private void Agent_ContinueEvent(string id, string next_operate)
        {
            if (_manager is WorkflowDebuggerManager)
            {
                var m = _manager as WorkflowDebuggerManager;
                m.Continue();
            }
        }

        private void Agent_UpdateAgentConfigEvent(string id, string key, string val)
        {
            _logger.Debug($"update_agent_config,id={id},key={key},val={val}");
            if (_manager != null)
            {
                _manager.SetConfig(key, val);
            }
        }

        private void Agent_StartEvent(bool is_in_debugging_state, JObject debug_json_cfg, JObject command_line_json_cfg, string name, string version, string mainXamlPath
            , List<string> loadAssemblyFromList, List<string> assemblyResolveDllList, string projectPath)
        {
            var desc = is_in_debugging_state ? "调试" : "执行";
            _logger.Debug($"收到start指令，开始{desc}工作流：" + mainXamlPath);

            this._assemblyResolveDllList = assemblyResolveDllList;

            SharedObject.Instance.ProjectPath = projectPath;
            SharedObject.Instance.OutputEvent -= Instance_OutputEvent;
            SharedObject.Instance.OutputEvent += Instance_OutputEvent;

            foreach (var dll_file in loadAssemblyFromList)
            {
                try
                {
                    var checkPath = Path.Combine(SharedObject.Instance.ApplicationCurrentDirectory, Path.GetFileName(dll_file));
                    if (System.IO.File.Exists(checkPath))
                    {
                        continue;//避免加载重复的DLL，主程序所在目录下存在同名dll，忽略加载
                    }

                    Assembly.LoadFrom(dll_file);
                }
                catch (Exception)
                {

                }
            }

            if (is_in_debugging_state)
            {
                _logger.Debug("初始化调试管理器");
                _manager = new WorkflowDebuggerManager(_agent, name, version, mainXamlPath);
                initDebugJsonConfig(debug_json_cfg);
            }
            else
            {
                _manager = new WorkflowRunManager(_agent, name, version, mainXamlPath);
            }

            if (_rpaFlag == "RPACommandLine")
            {
                initCommandLineJsonConfig(command_line_json_cfg);
            }

            Directory.SetCurrentDirectory(SharedObject.Instance.ProjectPath);

            _manager.Run();
        }

        private void Instance_NotifyEvent(string notification, string notificationDetails)
        {
            _manager.RedirectNotification(notification, notificationDetails);
        }

        private void Instance_OutputEvent(SharedObject.enOutputType type, string msg, string msgDetails)
        {
            RedirectLogToOutputWindow(type, msg, msgDetails);
        }

        private void initDebugJsonConfig(JObject json_cfg)
        {
            _manager.SetConfig("next_operate", json_cfg["next_operate"].ToString());
            _manager.SetConfig("slow_step_speed_ms", json_cfg["slow_step_speed_ms"].ToString());
            _manager.SetConfig("is_log_activities", json_cfg["is_log_activities"].ToString());
            _manager.SetConfig("activity_id_json_array", json_cfg["activity_id_json_array"].ToString());
            _manager.SetConfig("breakpoint_id_json_array", json_cfg["breakpoint_id_json_array"].ToString());
            _manager.SetConfig("tracker_vars", json_cfg["tracker_vars"].ToString());
        }

        private void initCommandLineJsonConfig(JObject json_cfg)
        {
            _manager.SetConfig("input_args", json_cfg["input"].ToString());
        }

        /// <summary>
        /// 重定向日志到输出窗口，通过IPC管道方式发送
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <param name="msgDetails"></param>
        private void RedirectLogToOutputWindow(SharedObject.enOutputType type, string msg, string msgDetails)
        {
            _manager.RedirectLogToOutputWindow(type, msg, msgDetails);
        }

        private void Agent_StopEvent()
        {
            _logger.Debug("收到stop指令，程序退出……");

            _quitTimer.Interval = TimeSpan.FromMilliseconds(300);
            _quitTimer.Tick += QuitTimer_Tick;
            _quitTimer.Start();
        }



        private void Agent_ExitEvent()
        {
            _logger.Debug("收到exit指令，程序退出……");

            _quitTimer.Interval = TimeSpan.FromMilliseconds(300);
            _quitTimer.Tick += QuitTimer_Tick;
            _quitTimer.Start();
        }


        //延时退出，防止用户点击停止调试按钮过快，有残留的退出指令影响下一次调试
        private void QuitTimer_Tick(object sender, EventArgs e)
        {
            _quitTimer.Tick -= QuitTimer_Tick;
            _logger.Debug("程序正式退出");
            Environment.Exit(0);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
#if RPA_EXECUTOR_CONSOLE_DEBUG
            Common.CloseConsole();
#endif
        }


        /// <summary>
        /// 未处理的异常捕获
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">参数</param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Common.InvokeOnUI(() =>
            {
                try
                {
                    _manager.OnUnhandledException("UI线程全局异常", e.Exception);

                    _logger.Error("UI线程全局异常");
                    _logger.Error(e.Exception);
                    e.Handled = true;
                }
                catch (Exception ex)
                {
                    _logger.Fatal("不可恢复的UI线程全局异常");
                    _logger.Fatal(ex);
                }
            });
        }

        /// <summary>
        /// 当前域异常捕获
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">参数</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Common.InvokeOnUI(() =>
            {
                try
                {
                    var exception = e.ExceptionObject as Exception;
                    if (exception != null)
                    {
                        _manager.OnUnhandledException("非UI线程全局异常", exception);

                        _logger.Error("非UI线程全局异常");
                        _logger.Error(exception);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Fatal("不可恢复的非UI线程全局异常");
                    _logger.Fatal(ex);
                }
            });
        }

        /// <summary>
        /// 当前域动态加载程序集处理函数
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="args">参数</param>
        /// <returns>解析并加载后的程序集</returns>
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.Name.Split(',')[0];

            var path = _assemblyResolveDllList.Where(item => System.IO.Path.GetFileNameWithoutExtension(item).Equals(name)).FirstOrDefault();

            if (System.IO.File.Exists(path))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(path);
                    return assembly;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                path = Path.Combine(SharedObject.Instance.ApplicationCurrentDirectory, name + ".dll");
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(path);
                        return assembly;
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }

                Trace.WriteLine(string.Format("********************{0} 无法找到相应的程序集********************", args.Name));
            }

            return null;
        }




    }
}
