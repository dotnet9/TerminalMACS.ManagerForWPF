using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using QuickApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace QuickApp.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private bool isPowerOn = false;
        /// <summary>
        /// 是否开机启动
        /// </summary>
        public bool IsPowerOn
        {
            get { return isPowerOn; }
            set { SetProperty(ref isPowerOn, value); }
        }

        /// <summary>
        /// 退出应用命令
        /// </summary>
        public ICommand RaiseExitCommand { get; private set; }

        /// <summary>
        /// 添加命令行命令
        /// </summary>
        public ICommand RaiseAddCmdCommand { get; private set; }

        /// <summary>
        /// 开机启动命令
        /// </summary>
        public ICommand RaisePowerOnCommand { get; private set; }

        public ShellViewModel()
        {
            IsPowerOn = ConfigHelper.IsPowerOn();
            RaiseAddCmdCommand = new DelegateCommand(RaiseAddCmdHandler);
            RaiseExitCommand = new DelegateCommand(RaiseExitHandler);
            RaisePowerOnCommand = new DelegateCommand(RaisePowerOnHandler);
        }

        /// <summary>
        /// 添加命令行
        /// </summary>
        private void RaiseAddCmdHandler()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "选择Exe文件";
                openFileDialog.Filter = "exe文件|*.exe";
                openFileDialog.FileName = string.Empty;
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.DefaultExt = "exe";
                if (openFileDialog.ShowDialog() == false)
                {
                    return;
                }
                string txtFile = openFileDialog.FileName;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 开机启动与取消切换
        /// </summary>
        private void RaisePowerOnHandler()
        {
            RegRun("QuickApp", IsPowerOn);
            ConfigHelper.SetIsPowerOn(IsPowerOn);
        }

        /// <summary>
        /// 开机启动和取消开机启动
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="f"></param>
        void RegRun(string appName, bool f)
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            bool b = false;
            foreach (string i in Run.GetValueNames())
            {
                if (i == appName)
                {
                    b = true;
                    break;
                }
            }
            try
            {
                if (f)
                {
                    Run.SetValue(appName, "\"" + Process.GetCurrentProcess().MainModule.FileName + "\"");
                }
                else
                {
                    Run.DeleteValue(appName);
                }
            }
            catch
            { }
            HKCU.Close();
        }

        /// <summary>
        /// 退出应用
        /// </summary>
        private void RaiseExitHandler()
        {
            App.Current.MainWindow.Close();
        }
    }
}
