using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json.Linq;
using NLog;
using RPA.Interfaces.Nupkg;
using RPA.Interfaces.Project;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ExportViewModel : ViewModelBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public Window Window { get; set; }

        private IProjectManagerService _projectManagerService;
        private IPackageExportService _packageExportService;
        private IPackageExportSettingsService _packageExportSettingsService;

        /// <summary>
        /// Initializes a new instance of the ExportViewModel class.
        /// </summary>
        public ExportViewModel(IProjectManagerService projectManagerService,IPackageExportService packageExportService
            , IPackageExportSettingsService packageExportSettingsService)
        {
            _projectManagerService = projectManagerService;
            _packageExportService = packageExportService;
            _packageExportSettingsService = packageExportSettingsService;

            InitHistory();
            InitVersionInfo();
        }

        private void InitHistory()
        {
            CustomLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var lastExportDir = _packageExportSettingsService.GetLastExportDir();
            if (System.IO.Directory.Exists(lastExportDir))
            {
                CustomLocation = lastExportDir;
            }
            else
            {
                CustomLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            var exportDirHistoryList = _packageExportSettingsService.GetExportDirHistoryList();
            if (exportDirHistoryList != null)
            {
                foreach (var item in exportDirHistoryList)
                {
                    CustomLocations.Add(item);
                }
            }
        }

        private void InitVersionInfo()
        {
            CurrentProjectVersion = _projectManagerService.CurrentProjectJsonConfig.projectVersion;

            Version currentVersion = new Version(CurrentProjectVersion);
            Version newVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build + 1);
            NewProjectVersion = newVersion.ToString();
        }


        /// <summary>
        /// The <see cref="IsExportToLocalRobot" /> property's name.
        /// </summary>
        public const string IsExportToLocalRobotPropertyName = "IsExportToLocalRobot";

        private bool _isExportToLocalRobotProperty = true;

        /// <summary>
        /// Sets and gets the IsExportToLocalRobot property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsExportToLocalRobot
        {
            get
            {
                return _isExportToLocalRobotProperty;
            }

            set
            {
                if (_isExportToLocalRobotProperty == value)
                {
                    return;
                }

                _isExportToLocalRobotProperty = value;
                RaisePropertyChanged(IsExportToLocalRobotPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="CustomLocation" /> property's name.
        /// </summary>
        public const string CustomLocationPropertyName = "CustomLocation";

        private string _customLocationProperty = "";

        /// <summary>
        /// 自定义位置
        /// </summary>
        public string CustomLocation
        {
            get
            {
                return _customLocationProperty;
            }

            set
            {
                if (_customLocationProperty == value)
                {
                    return;
                }

                _customLocationProperty = value;
                RaisePropertyChanged(CustomLocationPropertyName);

                IsCustomLocationCorrect = System.IO.Directory.Exists(value);
                if (!IsCustomLocationCorrect)
                {
                    CustomLocationValidatedWrongTip = "指定的路径不存在";
                }
            }
        }

        /// <summary>
        /// The <see cref="CustomLocations" /> property's name.
        /// </summary>
        public const string CustomLocationsPropertyName = "CustomLocations";

        private ObservableCollection<string> _customLocationsProperty = new ObservableCollection<string>();

        /// <summary>
        /// 自定义位置列表，存放可能的历史记录
        /// </summary>
        public ObservableCollection<string> CustomLocations
        {
            get
            {
                return _customLocationsProperty;
            }

            set
            {
                if (_customLocationsProperty == value)
                {
                    return;
                }

                _customLocationsProperty = value;
                RaisePropertyChanged(CustomLocationsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsCustomLocationCorrect" /> property's name.
        /// </summary>
        public const string IsCustomLocationCorrectPropertyName = "IsCustomLocationCorrect";

        private bool _isCustomLocationCorrectProperty = false;

        /// <summary>
        /// 是否默认位置正确
        /// </summary>
        public bool IsCustomLocationCorrect
        {
            get
            {
                return _isCustomLocationCorrectProperty;
            }

            set
            {
                if (_isCustomLocationCorrectProperty == value)
                {
                    return;
                }

                _isCustomLocationCorrectProperty = value;
                RaisePropertyChanged(IsCustomLocationCorrectPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CustomLocationValidatedWrongTip" /> property's name.
        /// </summary>
        public const string CustomLocationValidatedWrongTipPropertyName = "CustomLocationValidatedWrongTip";

        private string _customLocationValidatedWrongTipProperty = "";

        /// <summary>
        /// 自定义位置校验错误提示
        /// </summary>
        public string CustomLocationValidatedWrongTip
        {
            get
            {
                return _customLocationValidatedWrongTipProperty;
            }

            set
            {
                if (_customLocationValidatedWrongTipProperty == value)
                {
                    return;
                }

                _customLocationValidatedWrongTipProperty = value;
                RaisePropertyChanged(CustomLocationValidatedWrongTipPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ReleaseNotes" /> property's name.
        /// </summary>
        public const string ReleaseNotesPropertyName = "ReleaseNotes";

        private string _releaseNotesProperty = "";

        /// <summary>
        /// 发布说明
        /// </summary>
        public string ReleaseNotes
        {
            get
            {
                return _releaseNotesProperty;
            }

            set
            {
                if (_releaseNotesProperty == value)
                {
                    return;
                }

                _releaseNotesProperty = value;
                RaisePropertyChanged(ReleaseNotesPropertyName);
            }
        }

        private RelayCommand _browserFolderCommand;

        /// <summary>
        /// 浏览目录
        /// </summary>
        public RelayCommand BrowserFolderCommand
        {
            get
            {
                return _browserFolderCommand
                    ?? (_browserFolderCommand = new RelayCommand(
                    () =>
                    {
                        string dst_dir = "";
                        if (CommonDialog.ShowSelectDirDialog("请选择一个位置来发布项目", ref dst_dir))
                        {
                            CustomLocation = dst_dir;
                        }
                    }));
            }
        }

        /// <summary>
        /// The <see cref="CurrentProjectVersion" /> property's name.
        /// </summary>
        public const string CurrentProjectVersionPropertyName = "CurrentProjectVersion";

        private string _currentProjectVersionProperty = "";

        /// <summary>
        /// 当前项目版本信息 
        /// </summary>
        public string CurrentProjectVersion
        {
            get
            {
                return _currentProjectVersionProperty;
            }

            set
            {
                if (_currentProjectVersionProperty == value)
                {
                    return;
                }

                _currentProjectVersionProperty = value;
                RaisePropertyChanged(CurrentProjectVersionPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewProjectVersion" /> property's name.
        /// </summary>
        public const string NewProjectVersionPropertyName = "NewProjectVersion";

        private string _newProjectVersionProperty = "";

        /// <summary>
        /// 新项目版本信息
        /// </summary>
        public string NewProjectVersion
        {
            get
            {
                return _newProjectVersionProperty;
            }

            set
            {
                if (_newProjectVersionProperty == value)
                {
                    return;
                }

                _newProjectVersionProperty = value;
                RaisePropertyChanged(NewProjectVersionPropertyName);

                IsNewProjectVersionCorrect = !string.IsNullOrWhiteSpace(value);
                if (!IsNewProjectVersionCorrect)
                {
                    NewProjectVersionValidatedWrongTip = "版本号不能为空";
                    return;
                }

                try
                {
                    var ver = new Version(value);
                    if (ver.Major >= 0 && ver.Minor >= 0 && ver.Build >= 0 && ver.Revision < 0)
                    {
                        IsNewProjectVersionCorrect = true;
                    }
                    else
                    {
                        IsNewProjectVersionCorrect = false;
                        NewProjectVersionValidatedWrongTip = "版本号须为a.b.c形式";
                    }
                }
                catch (Exception)
                {
                    IsNewProjectVersionCorrect = false;
                    NewProjectVersionValidatedWrongTip = "版本号非法";
                }

            }
        }

        /// <summary>
        /// The <see cref="NewProjectVersionValidatedWrongTip" /> property's name.
        /// </summary>
        public const string NewProjectVersionValidatedWrongTipPropertyName = "NewProjectVersionValidatedWrongTip";

        private string _newProjectVersionValidatedWrongTipProperty = "";

        /// <summary>
        /// 新项目版本校验错误提示信息
        /// </summary>
        public string NewProjectVersionValidatedWrongTip
        {
            get
            {
                return _newProjectVersionValidatedWrongTipProperty;
            }

            set
            {
                if (_newProjectVersionValidatedWrongTipProperty == value)
                {
                    return;
                }

                _newProjectVersionValidatedWrongTipProperty = value;
                RaisePropertyChanged(NewProjectVersionValidatedWrongTipPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsNewProjectVersionCorrect" /> property's name.
        /// </summary>
        public const string IsNewProjectVersionCorrectPropertyName = "IsNewProjectVersionCorrect";

        private bool _isNewProjectVersionCorrectProperty = false;

        /// <summary>
        /// 新项目版本是否正确
        /// </summary>
        public bool IsNewProjectVersionCorrect
        {
            get
            {
                return _isNewProjectVersionCorrectProperty;
            }

            set
            {
                if (_isNewProjectVersionCorrectProperty == value)
                {
                    return;
                }

                _isNewProjectVersionCorrectProperty = value;
                RaisePropertyChanged(IsNewProjectVersionCorrectPropertyName);

            }
        }

        private RelayCommand _cancelCommand;

        /// <summary>
        /// 取消
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                    ?? (_cancelCommand = new RelayCommand(
                    () =>
                    {
                        Window.Close();
                    }));
            }
        }

        private RelayCommand _okCommand;

        /// <summary>
        /// Gets the OkCommand.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand
                    ?? (_okCommand = new RelayCommand(
                    () =>
                    {
                        Window.Hide();

                        var nupkgLocation = "";

                        if (IsExportToLocalRobot)
                        {
                            nupkgLocation = AppPathConfig.RobotPackagesDir;
                        }
                        else
                        {
                            nupkgLocation = CustomLocation;
                        }

                        try
                        {
                            var publishAuthors = Environment.UserName;
                            var publishVersion = NewProjectVersion;
                            var projectPath = _projectManagerService.CurrentProjectPath;
                            var publishId = _projectManagerService.CurrentProjectJsonConfig.name;
                            var publishDesc = string.IsNullOrWhiteSpace(_projectManagerService.CurrentProjectJsonConfig.description) ? "N/A" : _projectManagerService.CurrentProjectJsonConfig.description;

                            var dependenciesList = new List<NugetPackageItem>();
                            foreach (JProperty jp in (JToken)_projectManagerService.CurrentProjectJsonConfig.dependencies)
                            {
                                dependenciesList.Add(new NugetPackageItem(jp.Name, (string)jp.Value));
                            }


                            //提前更新当前项目的projectVersion为新版本号
                            UpdateProjectVersion();

                            _packageExportService.Init(publishId, publishVersion, publishDesc, publishAuthors, publishAuthors, ReleaseNotes);
                            _packageExportService.WithDependencies(dependenciesList);
                            _packageExportService.WithFiles(projectPath, $"{ProjectConstantConfig.ProjectLocalDirectoryName}/**");

                            var outputPath = _packageExportService.ExportToDir(nupkgLocation);

                            _packageExportSettingsService.AddToExportDirHistoryList(nupkgLocation);

                            _packageExportSettingsService.SetLastExportDir(nupkgLocation);//记住导出位置

                            //弹窗生成成功
                            if (System.IO.File.Exists(outputPath))
                            {
                                var info = string.Format("项目导出成功。\n名称：{0}\n版本：{1}\n位置：{2}\n"
                                    , _projectManagerService.CurrentProjectJsonConfig.name, publishVersion, nupkgLocation);
                                CommonMessageBox.ShowInformation(info);
                            }
                            else
                            {
                                throw new Exception("找不到导出后的包");
                            }
                        }
                        catch (Exception err)
                        {
                            SharedObject.Instance.Output(SharedObject.enOutputType.Error, "导出项目失败！", err);
                            _logger.Debug(err);
                            CommonMessageBox.ShowWarning("导出项目失败！");
                        }

                        Window.Close();
                    },
                    () => IsCustomLocationCorrect && IsNewProjectVersionCorrect));
            }
        }

        private void UpdateProjectVersion()
        {
            //json更新
            _projectManagerService.CurrentProjectJsonConfig.projectVersion = NewProjectVersion;
            _projectManagerService.SaveCurrentProjectJson();
        }
    }
}