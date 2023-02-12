using GongSolutions.Wpf.DragDrop;
using RPA.Interfaces.Activities;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Nupkg;
using RPA.Interfaces.Project;
using RPA.Interfaces.Workflow;
using RPA.Services.Activities;
using RPA.Services.AppDomains;
using RPA.Services.Nupkg;
using RPA.Services.Project;
using RPA.Services.Workflow;
using RPAStudio.DragDrop;
using RPAStudio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPAStudio.Service
{
    class RPAStudioServiceRegistry : AppServiceRegistry
    {
        public override void OnRegisterServices()
        {
            _serviceLocator.RegisterTypeSingleton<IProjectManagerService, ProjectManagerService>();
            _serviceLocator.RegisterTypeSingleton<IPackageRepositoryService, PackageRepositoryService>();
            _serviceLocator.RegisterTypeSingleton<IWorkflowStateService, WorkflowStateService>();
            _serviceLocator.RegisterTypeSingleton<IActivityMountService, ActivityMountService>();
            _serviceLocator.RegisterTypeSingleton<IAppDomainControllerService, AppDomainControllerService>();
            _serviceLocator.RegisterTypeSingleton<IProjectConfigFileService, ProjectConfigFileService>();
            _serviceLocator.RegisterTypeSingleton<IPackageIdentityService, PackageIdentityService>();
            _serviceLocator.RegisterTypeSingleton<IWorkflowBreakpointsServiceProxy, WorkflowBreakpointsServiceProxy>();
            _serviceLocator.RegisterTypeSingleton<IRecentProjectsConfigService, RecentProjectsConfigService>();
            _serviceLocator.RegisterTypeSingleton<IWorkflowDesignerCollectServiceProxy, WorkflowDesignerCollectServiceProxy>();
            _serviceLocator.RegisterTypeSingleton<IActivityFavoritesService, ActivityFavoritesService>();
            _serviceLocator.RegisterTypeSingleton<IActivityRecentService, ActivityRecentService>();
            _serviceLocator.RegisterTypeSingleton<ISystemActivityIconService, SystemActivityIconService>();
            _serviceLocator.RegisterTypeSingleton<IPackageExportSettingsService, PackageExportSettingsService>();
            _serviceLocator.RegisterTypeSingleton<IWorkflowDebugService, WorkflowDebugService>();
            _serviceLocator.RegisterTypeSingleton<IWorkflowRunService, WorkflowRunService>();

            _serviceLocator.RegisterType<IPackageImportService, PackageImportService>();
            _serviceLocator.RegisterType<IPackageExportService, PackageExportService>();
            _serviceLocator.RegisterType<IAppDomainContainerService, AppDomainContainerService>();
            _serviceLocator.RegisterType<IActivitiesServiceProxy, ActivitiesServiceProxy>();
            _serviceLocator.RegisterType<IDragSource, ProjectItemDragHandler>();
            _serviceLocator.RegisterType<IDropTarget, ProjectItemDropHandler>();
            _serviceLocator.RegisterType<IWorkflowDesignerServiceProxy, WorkflowDesignerServiceProxy>();
            
            

        }

        public override void OnRegisterViewModels()
        {
            _serviceLocator.RegisterTypeSingleton<MainViewModel>();
            _serviceLocator.RegisterTypeSingleton<DocksViewModel>();
            _serviceLocator.RegisterTypeSingleton<ProjectViewModel>();
            _serviceLocator.RegisterTypeSingleton<OutputViewModel>();
            _serviceLocator.RegisterTypeSingleton<ActivitiesViewModel>();
            _serviceLocator.RegisterTypeSingleton<OutputViewModel>();
            _serviceLocator.RegisterTypeSingleton<DocksViewModel>();
            _serviceLocator.RegisterTypeSingleton<ProjectViewModel>();
            _serviceLocator.RegisterTypeSingleton<ActivitiesViewModel>();
            _serviceLocator.RegisterTypeSingleton<PropertyViewModel>();
            _serviceLocator.RegisterTypeSingleton<OutlineViewModel>();
            _serviceLocator.RegisterTypeSingleton<SnippetsViewModel>();
            _serviceLocator.RegisterTypeSingleton<DebugViewModel>();

            _serviceLocator.RegisterType<StartPageViewModel>();
            _serviceLocator.RegisterType<ToolsPageViewModel>();
            _serviceLocator.RegisterType<SettingsPageViewModel>();
            _serviceLocator.RegisterType<AboutViewModel>();
            _serviceLocator.RegisterType<NewProjectViewModel>();
            _serviceLocator.RegisterType<RecentUsedProjectItemViewModel>();
            _serviceLocator.RegisterType<DesignerDocumentViewModel>();
            _serviceLocator.RegisterType<ProjectRootItemViewModel>();
            _serviceLocator.RegisterType<ProjectDirItemViewModel>();
            _serviceLocator.RegisterType<ProjectFileItemViewModel>();
            _serviceLocator.RegisterType<ProjectDependRootItemViewModel>();
            _serviceLocator.RegisterType<ProjectDependItemViewModel>();
            _serviceLocator.RegisterType<SnippetItemViewModel>();
            _serviceLocator.RegisterType<ActivityGroupItemViewModel>();
            _serviceLocator.RegisterType<ActivityLeafItemViewModel>();
            _serviceLocator.RegisterType<NewXamlFileViewModel>();
            _serviceLocator.RegisterType<RenameViewModel>();
            _serviceLocator.RegisterType<NewFolderViewModel>();
            _serviceLocator.RegisterType<ProjectSettingsViewModel>();
            _serviceLocator.RegisterType<MessageDetailsViewModel>();
            _serviceLocator.RegisterType<OutputListItemViewModel>();
            _serviceLocator.RegisterType<TrackerItemViewModel>();
            _serviceLocator.RegisterType<ExportViewModel>();
            _serviceLocator.RegisterType<RenameViewModel>();
            _serviceLocator.RegisterType<TrackerItemViewModel>();
        }
    }
}
