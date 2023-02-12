using NLog;
using RPA.Shared.Configs;
using RPARobot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARobot.Services
{
    public class RobotPathConfigService : IRobotPathConfigService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public string AppDataDir { get; set; }

        public string LogsDir { get; set; }

        public string ConfigDir { get; set; }

        public string PackagesDir { get; set; }

        public string ScreenRecorderDir { get; set; }

        public string ProgramDataPackagesDir { get; set; }

        public string ProgramDataInstalledPackagesDir { get; set; }

        public string AppProgramDataDir { get; set; }

        public RobotPathConfigService()
        {

        }

        public void InitDirs()
        {
            //localappdata目录初始化
            AppDataDir = AppPathConfig.StudioAppDataDir;

            LogsDir = AppPathConfig.StudioLogsDir;

            if (!System.IO.Directory.Exists(LogsDir))
            {
                System.IO.Directory.CreateDirectory(LogsDir);
            }


            PackagesDir = AppPathConfig.StudioPackagesDir;

            if (!System.IO.Directory.Exists(PackagesDir))
            {
                System.IO.Directory.CreateDirectory(PackagesDir);
            }

            //programdata目录下的robot的nupkg包安装相关目录初始化
            var commonApplicationData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            AppProgramDataDir = AppPathConfig.RobotAppDataDir;
            ProgramDataPackagesDir = AppPathConfig.RobotPackagesDir;//机器人默认读取nupkg包的位置
            ProgramDataInstalledPackagesDir = AppPathConfig.RobotInstalledPackagesDir;//机器人默认安装nupkg包的位置

            if (!System.IO.Directory.Exists(ProgramDataPackagesDir))
            {
                System.IO.Directory.CreateDirectory(ProgramDataPackagesDir);
            }

            if (!System.IO.Directory.Exists(ProgramDataInstalledPackagesDir))
            {
                System.IO.Directory.CreateDirectory(ProgramDataInstalledPackagesDir);
            }           
        }


    }
}
