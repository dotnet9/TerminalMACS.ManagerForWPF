using NLog;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Executor;
using RPA.Shared.Utils;
using RPARobot.Interfaces;
using RPARobot.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARobot.Services
{
    public class RunManagerService : IRunManagerService
    {
        private IServiceLocator _serviceLocator;

        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 包条目视图模型
        /// </summary>
        public PackageItemViewModel PackageItem { get; set; }

        /// <summary>
        /// xaml路径
        /// </summary>
        private string _xamlPath { get; set; }

        /// <summary>
        /// 判断运行过程中是否发生了异常
        /// </summary>
        public bool HasException { get; set; }

        /// <summary>
        /// rpa代理类
        /// </summary>
        private RPAExecutorController _controller = new RPAExecutorController();
        private string _guid;

        private List<string> _activitiesDllLoadFrom = new List<string>();
        private List<string> _dependentAssemblies = new List<string>();

        public event EventHandler BeginRunEvent;
        public event EventHandler EndRunEvent;

        public RunManagerService(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _controller.ExceptionEvent += _controller_ExceptionEvent;
            _controller.CompleteEvent += _controller_CompleteEvent;
            _controller.LogEvent += _controller_LogEvent;
            _controller.InitRobot();
        }

        public void Init(PackageItemViewModel packageItem, string xamlPath, List<string> activitiesDllLoadFrom, List<string> dependentAssemblies)
        {
            PackageItem = packageItem;
            _xamlPath = xamlPath;

            _activitiesDllLoadFrom = activitiesDllLoadFrom;
            _dependentAssemblies = dependentAssemblies;
        }



        private void _controller_ExceptionEvent(string id, string title, string msg)
        {
            Common.InvokeOnUI(() =>
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, title, msg);
                CommonMessageBox.ShowError(title);
            });
        }


        private void _controller_CompleteEvent(string id, bool has_exception)
        {
            this.HasException = has_exception;
            EndRunEvent?.Invoke(this, EventArgs.Empty);

            _logger.Debug(string.Format("结束执行工作流文件 {0}", _xamlPath));
        }

        private void _controller_LogEvent(string id, string type, string msg, string msgDetails)
        {
            var t = (SharedObject.enOutputType)Enum.Parse(typeof(SharedObject.enOutputType), type);
            SharedObject.Instance.Output(t, msg, msgDetails);
        }




        /// <summary>
        /// 开始执行运行流程
        /// </summary>
        public void Run()
        {
            _logger.Debug(string.Format("开始执行工作流文件 {0} ……", _xamlPath));
            BeginRunEvent?.Invoke(this, EventArgs.Empty);

            var guid = RPAExecutorController.Guid();

            _guid = guid;

            var cfg = new RPAExecutorStartupConfig();
            cfg.Name = PackageItem.Name;
            cfg.Version = PackageItem.Version;
            cfg.MainXamlPath = _xamlPath;
            cfg.PipeName = RPAExecutorAgent.GetExecutorPipeName(guid);
            cfg.LoadAssemblyFromList = _activitiesDllLoadFrom;
            cfg.AssemblyResolveDllList = _dependentAssemblies;
            cfg.ProjectPath = SharedObject.Instance.ProjectPath;

            _controller.SetStartupConfig(guid, cfg);
            var arg = _controller.Start(guid);
            _logger.Debug($"+++++++++++++++++启动进程 RPAExecutor.exe+++++++++++++++++ 命令行参数:{arg}");
        }

        /// <summary>
        /// 停止工作流运行
        /// </summary>
        public void Stop()
        {
            _controller.Stop(_guid);
            _controller_CompleteEvent(_guid, false);
        }



        /// <summary>
        /// 输出到日志
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="msg">消息</param>
        /// <param name="msgDetails">详情</param>
        public void LogToOutputWindow(SharedObject.enOutputType type, string msg, string msgDetails)
        {
            //活动日志：type={0},msg={1},msgDetails={2}
            _logger.Debug(string.Format("活动日志：type={0},msg={1},msgDetails={2}", type, msg, msgDetails));
        }



    }
}