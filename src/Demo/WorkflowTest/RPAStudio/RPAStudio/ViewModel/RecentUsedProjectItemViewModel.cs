using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Interfaces.Project;
using RPA.Shared.Utils;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class RecentUsedProjectItemViewModel : ViewModelBase
    {
        private IProjectManagerService _projectManagerService;
        private IRecentProjectsConfigService _recentProjectsConfigService;

        /// <summary>
        /// Initializes a new instance of the RecentUsedProjectItemViewModel class.
        /// </summary>
        public RecentUsedProjectItemViewModel(IProjectManagerService projectManagerService
            , IRecentProjectsConfigService recentProjectsConfigService)
        {
            _projectManagerService = projectManagerService;
            _recentProjectsConfigService = recentProjectsConfigService;
        }


        /// <summary>
        /// The <see cref="ProjectOrder" /> property's name.
        /// </summary>
        public const string ProjectOrderPropertyName = "ProjectOrder";

        private int _projectOrderProperty = 0;

        /// <summary>
        /// Sets and gets the ProjectOrder property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ProjectOrder
        {
            get
            {
                return _projectOrderProperty;
            }

            set
            {
                if (_projectOrderProperty == value)
                {
                    return;
                }

                _projectOrderProperty = value;
                RaisePropertyChanged(ProjectOrderPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="ProjectName" /> property's name.
        /// </summary>
        public const string ProjectNamePropertyName = "ProjectName";

        private string _projectNameProperty = "";

        /// <summary>
        /// Sets and gets the ProjectName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectName
        {
            get
            {
                return _projectNameProperty;
            }

            set
            {
                if (_projectNameProperty == value)
                {
                    return;
                }

                _projectNameProperty = value;
                RaisePropertyChanged(ProjectNamePropertyName);
            }
        }


        /// <summary>
        /// The <see cref="ProjectHeader" /> property's name.
        /// </summary>
        public const string ProjectHeaderPropertyName = "ProjectHeader";

        private string _projectHeaderProperty = "";

        /// <summary>
        /// Sets and gets the ProjectHeader property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectHeader
        {
            get
            {
                return _projectHeaderProperty;
            }

            set
            {
                if (_projectHeaderProperty == value)
                {
                    return;
                }

                _projectHeaderProperty = value;
                RaisePropertyChanged(ProjectHeaderPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="ProjectDescription" /> property's name.
        /// </summary>
        public const string ProjectDescriptionPropertyName = "ProjectDescription";

        private string _projectDescriptionProperty = "";

        /// <summary>
        /// Sets and gets the ProjectDescription property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectDescription
        {
            get
            {
                return _projectDescriptionProperty;
            }

            set
            {
                if (_projectDescriptionProperty == value)
                {
                    return;
                }

                _projectDescriptionProperty = value;
                RaisePropertyChanged(ProjectDescriptionPropertyName);
            }
        }







        /// <summary>
        /// The <see cref="ProjectConfigFilePath" /> property's name.
        /// </summary>
        public const string ProjectConfigFilePathPropertyName = "ProjectConfigFilePath";

        private string _projectConfigFilePathProperty = "";

        /// <summary>
        /// Sets and gets the ProjectConfigFilePath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectConfigFilePath
        {
            get
            {
                return _projectConfigFilePathProperty;
            }

            set
            {
                if (_projectConfigFilePathProperty == value)
                {
                    return;
                }

                _projectConfigFilePathProperty = value;
                RaisePropertyChanged(ProjectConfigFilePathPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="ProjectToolTip" /> property's name.
        /// </summary>
        public const string ProjectToolTipPropertyName = "ProjectToolTip";

        private string _projectToolTipProperty = "";

        /// <summary>
        /// Sets and gets the ProjectToolTip property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectToolTip
        {
            get
            {
                return _projectToolTipProperty;
            }

            set
            {
                if (_projectToolTipProperty == value)
                {
                    return;
                }

                _projectToolTipProperty = value;
                RaisePropertyChanged(ProjectToolTipPropertyName);
            }
        }


        private RelayCommand _openCommand;

        /// <summary>
        /// Gets the OpenCommand.
        /// </summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand
                    ?? (_openCommand = new RelayCommand(
                    async () =>
                    {
                        if (_projectManagerService.IsAlreadyOpened(ProjectConfigFilePath))
                        {
                            return;
                        }

                        if (_projectManagerService.CloseCurrentProject())
                        {
                            var ret = System.IO.File.Exists(ProjectConfigFilePath);
                            if (!ret)
                            {
                                if (CommonMessageBox.ShowQuestion(string.Format("无法打开项目配置文件{0}，是否从最近项目列表中移除该条目？", ProjectConfigFilePath)))
                                {
                                    _recentProjectsConfigService.Remove(ProjectConfigFilePath);
                                }
                            }
                            else
                            {
                                await _projectManagerService.OpenProject(ProjectConfigFilePath);
                            }
                        }
                    }));
            }
        }


        private RelayCommand _openDirCommand;

        /// <summary>
        /// 打开所在目录
        /// </summary>
        public RelayCommand OpenDirCommand
        {
            get
            {
                return _openDirCommand
                    ?? (_openDirCommand = new RelayCommand(
                    () =>
                    {
                        Common.LocateDirInExplorer(System.IO.Path.GetDirectoryName(ProjectConfigFilePath));
                    }));
            }
        }


        private RelayCommand _removeFromRecentUsedProjectsCommand;

        /// <summary>
        /// Gets the RemoveFromRecentUsedProjectsCommand.
        /// </summary>
        public RelayCommand RemoveFromRecentUsedProjectsCommand
        {
            get
            {
                return _removeFromRecentUsedProjectsCommand
                    ?? (_removeFromRecentUsedProjectsCommand = new RelayCommand(
                    () =>
                    {
                        _recentProjectsConfigService.Remove(ProjectConfigFilePath);
                    }));
            }
        }

    }
}