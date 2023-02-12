using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Configs
{
    public static class ProjectConstantConfig
    {
        public const string XamlFileExtension = ".xaml";

        public const string ProjectCreateName = "空白流程";

        public const string MainXamlFileName = "Main" + XamlFileExtension;

        public const string ProjectConfigFileName = "project";
        public const string ProjectConfigFileExtension = ".rpa";
        public const string ProjectConfigFileNameWithSuffix = ProjectConfigFileName + ProjectConfigFileExtension;

        public const string OfflinePackagesName = "OfflinePackages";

        public const string ProjectDefaultDependentPackagesMatchRegex = @".*\.Activities$";

        public const string ProjectLocalDirectoryName = ".local";

        public const string ProjectSettingsFileName = "ProjectSettings.json";

        public const int RecentProjectsMaxRecordCount = 30;

        public const string ProjectActivitiesAssemblyMatchRegex = @".*\.Activities.dll$";

        public const string ScreenshotsPath = ".screenshots";

        public const int ActivitiesRecentGroupMaxRecordCount = 30;

        public const int ExportDirHistoryMaxRecordCount = 10;
    }
}
