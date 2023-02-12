using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Shared.Utils;
using RPAStudio.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class StartPageViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;
        private IRecentProjectsConfigService _recentProjectsConfigService;

        /// <summary>
        /// Initializes a new instance of the StartPageViewModel class.
        /// </summary>
        public StartPageViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _recentProjectsConfigService = _serviceLocator.ResolveType<IRecentProjectsConfigService>();

            reloadRecentProjects();

            _recentProjectsConfigService.ChangeEvent += _recentProjectsConfigService_ChangeEvent;
        }

        private void reloadRecentProjects()
        {
            var list = _recentProjectsConfigService.Load();
            UpdateRecentUsedProjects(list);
        }

        private void _recentProjectsConfigService_ChangeEvent(object sender, EventArgs e)
        {
            reloadRecentProjects();
        }

        public void UpdateRecentUsedProjects(List<object> list)
        {
            //最近项目列表更新
            RecentUsedProjects.Clear();

            foreach (dynamic item in list)
            {
                //item为ExpandoObject类型
                var name = item.Name;
                var description = item.Description;
                var filePath = item.FilePath;

                var itemVM = _serviceLocator.ResolveType<RecentUsedProjectItemViewModel>();
                itemVM.ProjectOrder = list.IndexOf(item) + 1;
                itemVM.ProjectName = name;
                itemVM.ProjectConfigFilePath = filePath;
                itemVM.ProjectDescription = description;
                itemVM.ProjectToolTip = $"描述：{description}\n路径：{filePath}";

                itemVM.ProjectHeader = $"{itemVM.ProjectOrder}  {itemVM.ProjectName}";
                RecentUsedProjects.Add(itemVM);

            }
        }


        /// <summary>
        /// The <see cref="RecentUsedProjects" /> property's name.
        /// </summary>
        public const string RecentUsedProjectsPropertyName = "RecentUsedProjects";

        private ObservableCollection<RecentUsedProjectItemViewModel> _recentUsedProjectsProperty = new ObservableCollection<RecentUsedProjectItemViewModel>();

        /// <summary>
        /// Sets and gets the RecentUsedProjects property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<RecentUsedProjectItemViewModel> RecentUsedProjects
        {
            get
            {
                return _recentUsedProjectsProperty;
            }

            set
            {
                if (_recentUsedProjectsProperty == value)
                {
                    return;
                }

                _recentUsedProjectsProperty = value;
                RaisePropertyChanged(RecentUsedProjectsPropertyName);
            }
        }

        private RelayCommand _newProjectCommand;

        /// <summary>
        /// Gets the NewProjectCommand.
        /// </summary>
        public RelayCommand NewProjectCommand
        {
            get
            {
                return _newProjectCommand
                    ?? (_newProjectCommand = new RelayCommand(
                    () =>
                    {
                        //弹出新建项目对话框
                        var window = new NewProjectWindow();
                        var vm = window.DataContext as NewProjectViewModel;
                        vm.Window = window;
                        CommonWindow.ShowDialog(window);
                    }));
            }
        }




    }
}