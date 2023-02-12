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
    public class PackageService : IPackageService
    {
        private MainViewModel _mainViewModel;

        public PackageService(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }


        /// <summary>
        /// 运行指定名称和版本的包（停止其它正在运行的包）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="version">版本</param>
        public void Run(string name, string version)
        {
            Common.InvokeOnUI(() =>
            {
                string runningName, runningVersion;
                bool existRun = GetRunningPackage(out runningName, out runningVersion);

                if (existRun)
                {
                    if (runningName == name && runningVersion == version)
                    {
                        return;
                    }
                    else
                    {
                        StopCurrentRun();
                    }
                }


                RealRun(name, version);
            });

        }

        /// <summary>
        /// 实际的运行流程
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="version">版本</param>
        private void RealRun(string name, string version)
        {
            foreach (var item in _mainViewModel.PackageItems)
            {
                if (item.Name == name && item.Version == version)
                {
                    if (item.IsNeedUpdate)
                    {
                        item.UpdateCommand.Execute(null);
                    }

                    item.StartCommand.Execute(null);

                    Common.InvokeOnUI(() =>
                    {
                        _mainViewModel.RefreshAllPackages();
                    });

                    break;
                }
            }
        }

        /// <summary>
        /// 获取当前正在运行的包
        /// </summary>
        /// <param name="name">当前正在运行的包名</param>
        /// <param name="version">当前正在运行的包版本</param>
        /// <returns>是否有正在运行的包</returns>
        private bool GetRunningPackage(out string name, out string version)
        {
            foreach (var item in _mainViewModel.PackageItems)
            {
                if (item.IsRunning)
                {
                    name = item.Name;
                    version = item.Version;

                    return true;
                }
            }

            name = "";
            version = "";
            return false;
        }

        /// <summary>
        /// 停止当前正在运行的包
        /// </summary>
        private void StopCurrentRun()
        {
            _mainViewModel.StopCommand.Execute(null);
        }



    }
}
