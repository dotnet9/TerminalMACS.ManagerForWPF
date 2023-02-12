using NLog;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Shared.Configs
{
    public class AppPathConfig
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public static string StudioName = "RPAStudio";
        

        public static string StudioAppDataDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\"+ StudioName;
        public static string StudioLogsDir = StudioAppDataDir + @"\Logs";
        public static string StudioConfigsDir = StudioAppDataDir + @"\Configs";
        public static string StudioPluginsDir = StudioAppDataDir + @"\Plugins";
        public static string StudioPackagesDir = StudioAppDataDir + @"\Packages";

        public static string RobotName = "RPARobot";
        public static string RobotAppDataDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + @"\" + RobotName;
        public static string RobotPackagesDir = RobotAppDataDir + @"\Packages";
        public static string RobotInstalledPackagesDir = RobotAppDataDir + @"\InstalledPackages";

        public static string StudioUserConfigXml = StudioConfigsDir + @"\RPAStudio.User.xml";

        public static string RecentProjectsXml = StudioConfigsDir + @"\RecentProjects.xml";

        public static string RecentExportPathsXml = StudioConfigsDir + @"\RecentExportPaths.xml";

        public static string FavoriteActivitiesXml = StudioConfigsDir + @"\FavoriteActivities.xml";

        public static string RecentActivitiesXml = StudioConfigsDir + @"\RecentActivities.xml";

        public static string CodeSnippetsXml = StudioConfigsDir + @"\CodeSnippets.xml"; 

        public static string DefaultProjectsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + StudioName + @"\Projects";

        private static bool StudioUserConfigExport()
        {
            var fromResFile = $"pack://application:,,,/RPA.Resources;Component/Configs/RPAStudio.User.xml";
            var toFile = StudioUserConfigXml;

            if (!System.IO.File.Exists(toFile))
            {
                return Common.WriteResourceContentByUri(toFile, fromResFile);
            }

            return true;
        }

        private static bool RecentProjectsConfigExport()
        {
            var fromResFile = $"pack://application:,,,/RPA.Resources;Component/Configs/RecentProjects.xml";
            var toFile = RecentProjectsXml;

            if (!System.IO.File.Exists(toFile))
            {
                return Common.WriteResourceContentByUri(toFile, fromResFile);
            }

            return true;
        }

        private static bool RecentExportPathsConfigExport()
        {
            var fromResFile = $"pack://application:,,,/RPA.Resources;Component/Configs/RecentExportPaths.xml";
            var toFile = RecentExportPathsXml;

            if (!System.IO.File.Exists(toFile))
            {
                return Common.WriteResourceContentByUri(toFile, fromResFile);
            }

            return true;
        }

        private static bool RecentActivitiesConfigExport()
        {
            var fromResFile = $"pack://application:,,,/RPA.Resources;Component/Configs/RecentActivities.xml";
            var toFile = RecentActivitiesXml;

            if (!System.IO.File.Exists(toFile))
            {
                return Common.WriteResourceContentByUri(toFile, fromResFile);
            }

            return true;
        }

        private static bool FavoriteActivitiesConfigExport()
        {
            var fromResFile = $"pack://application:,,,/RPA.Resources;Component/Configs/FavoriteActivities.xml";
            var toFile = FavoriteActivitiesXml;

            if (!System.IO.File.Exists(toFile))
            {
                return Common.WriteResourceContentByUri(toFile, fromResFile);
            }

            return true;
        }

        private static bool CodeSnippetsConfigExport()
        {
            var fromResFile = $"pack://application:,,,/RPA.Resources;Component/Configs/CodeSnippets.xml";
            var toFile = CodeSnippetsXml;

            if (!System.IO.File.Exists(toFile))
            {
                return Common.WriteResourceContentByUri(toFile, fromResFile);
            }

            return true;
        }



        public static void InitConfigs()
        {
            Common.MakeSureDirectoryExists(DefaultProjectsDir);
            Common.MakeSureDirectoryExists(StudioConfigsDir);

            StudioUserConfigExport();
            RecentProjectsConfigExport();
            RecentExportPathsConfigExport();
            RecentActivitiesConfigExport();
            FavoriteActivitiesConfigExport();
            CodeSnippetsConfigExport();
        }
    }
}
