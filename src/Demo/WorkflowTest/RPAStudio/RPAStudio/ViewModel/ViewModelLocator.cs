using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using RPAStudio.Boot;
using RPAStudio.Service;

namespace RPAStudio.ViewModel
{
    public class ViewModelLocator
    {
        private AppServiceRegistry _locator = AppBoot.AppServiceRegistry;

        public ViewModelLocator()
        {

        }

        public MainViewModel Main
        {
            get
            {
                return _locator.ResolveType<MainViewModel>();
            }
        }

        public DocksViewModel Docks
        {
            get
            {
                return _locator.ResolveType<DocksViewModel>();
            }
        }


        public OutputViewModel Output
        {
            get
            {
                return _locator.ResolveType<OutputViewModel>();
            }
        }

        public StartPageViewModel StartPage
        {
            get
            {
                return _locator.ResolveType<StartPageViewModel>();
            }
        }

        public SettingsPageViewModel SettingsPage
        {
            get
            {
                return _locator.ResolveType<SettingsPageViewModel>();
            }
        }

        public AboutViewModel AboutPage
        {
            get
            {
                return _locator.ResolveType<AboutViewModel>();
            }
        }

        public ToolsPageViewModel ToolsPage
        {
            get
            {
                return _locator.ResolveType<ToolsPageViewModel>();
            }
        }


        public NewProjectViewModel NewProject
        {
            get
            {
                return _locator.ResolveType<NewProjectViewModel>();
            }
        }

        public ProjectViewModel Project
        {
            get
            {
                return _locator.ResolveType<ProjectViewModel>();
            }
        }

        public ActivitiesViewModel Activities
        {
            get
            {
                return _locator.ResolveType<ActivitiesViewModel>();
            }
        }


        public PropertyViewModel Property
        {
            get
            {
                return _locator.ResolveType<PropertyViewModel>();
            }
        }

        public OutlineViewModel Outline
        {
            get
            {
                return _locator.ResolveType<OutlineViewModel>();
            }
        }

        public SnippetsViewModel Snippets
        {
            get
            {
                return _locator.ResolveType<SnippetsViewModel>();
            }
        }

        public DebugViewModel Debug
        {
            get
            {
                return _locator.ResolveType<DebugViewModel>();
            }
        }

        public ExportViewModel Export
        {
            get
            {
                return _locator.ResolveType<ExportViewModel>();
            }
        }

        public NewXamlFileViewModel NewXamlFile
        {
            get
            {
                return _locator.ResolveType<NewXamlFileViewModel>();
            }
        }


        public RenameViewModel Rename
        {
            get
            {
                return _locator.ResolveType<RenameViewModel>();
            }
        }

        public NewFolderViewModel NewFolder
        {
            get
            {
                return _locator.ResolveType<NewFolderViewModel>();
            }
        }


        public ProjectSettingsViewModel ProjectSettings
        {
            get
            {
                return _locator.ResolveType<ProjectSettingsViewModel>();
            }
        }

        public MessageDetailsViewModel MessageDetails
        {
            get
            {
                return _locator.ResolveType<MessageDetailsViewModel>();
            }
        }


    }
}