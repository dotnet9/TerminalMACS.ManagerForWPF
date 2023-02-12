using ActiproSoftware.Windows.Controls.Docking;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Shared.Extensions;
using RPA.Shared.Utils;
using RPAStudio.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DocksViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;

        private IWorkflowStateService _workflowStateService;
        private IProjectManagerService _projectManagerService;

        public event EventHandler DocumentSelectChangeEvent;

        public DocksView View { get; set; }

        /// <summary>
        /// Initializes a new instance of the DocksViewModel class.
        /// </summary>
        public DocksViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();
            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();

            _projectManagerService.ProjectPreviewCloseEvent += _projectManagerService_ProjectPreviewCloseEvent;
        }



        private RelayCommand<RoutedEventArgs> _loadedCommand;

        /// <summary>
        /// 窗体加载完成后调用
        /// </summary>
        public RelayCommand<RoutedEventArgs> LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        View = (DocksView)p.Source;
                    }));
            }
        }


        private RelayCommand<DockingWindowsEventArgs> _windowsClosingCommandCommand;

        /// <summary>
        /// Gets the WindowClosingCommand.
        /// </summary>
        public RelayCommand<DockingWindowsEventArgs> WindowsClosingCommand
        {
            get
            {
                return _windowsClosingCommandCommand
                    ?? (_windowsClosingCommandCommand = new RelayCommand<DockingWindowsEventArgs>(
                    p =>
                    {
                        foreach (var dockingWindow in p.Windows)
                        {
                            var documentViewModel = dockingWindow.DataContext as DocumentViewModel;
                            if (documentViewModel != null)
                            {
                                bool isClosed = documentViewModel.CloseQuery();
                                if (!isClosed)
                                {
                                    p.Cancel = true;
                                }
                            }
                        }
                    }));
            }
        }



        /// <summary>
        /// The <see cref="IsAppDomainViewsVisible" /> property's name.
        /// </summary>
        public const string IsAppDomainViewsVisiblePropertyName = "IsAppDomainViewsVisible";

        private bool _isAppDomainViewsVisibleProperty = true;

        /// <summary>
        /// Sets and gets the IsAppDomainViewsVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsAppDomainViewsVisible
        {
            get
            {
                return _isAppDomainViewsVisibleProperty;
            }

            set
            {
                if (_isAppDomainViewsVisibleProperty == value)
                {
                    return;
                }

                _isAppDomainViewsVisibleProperty = value;
                RaisePropertyChanged(IsAppDomainViewsVisiblePropertyName);
            }
        }





        private void _projectManagerService_ProjectPreviewCloseEvent(object sender, CancelEventArgs e)
        {
            e.Cancel = !CloseAllQuery();
        }


        public void RaiseDocumentSelectChangeEvent(DocumentViewModel documentViewModel)
        {
            DocumentSelectChangeEvent?.Invoke(documentViewModel, EventArgs.Empty);
        }



        public DocumentViewModel SelectedDocument
        {
            get
            {
                foreach (var doc in Documents)
                {
                    if (doc.IsSelected)
                    {
                        return doc;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// The <see cref="Documents" /> property's name.
        /// </summary>
        public const string DocumentsPropertyName = "Documents";

        private ObservableCollection<DocumentViewModel> _documentsProperty = new ObservableCollection<DocumentViewModel>();

        /// <summary>
        /// 所有打开的文档
        /// </summary>
        public ObservableCollection<DocumentViewModel> Documents
        {
            get
            {
                return _documentsProperty;
            }

            set
            {
                if (_documentsProperty == value)
                {
                    return;
                }

                _documentsProperty = value;
                RaisePropertyChanged(DocumentsPropertyName);
            }
        }

        public DocumentViewModel NewDesignerDocument(string path)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(path);
            var doc = _serviceLocator.ResolveType<DesignerDocumentViewModel>();
            doc.Path = path;
            doc.RelativePath = Common.MakeRelativePath(_projectManagerService.CurrentProjectPath, path);
            doc.Title = name;
            doc.IsShowIcon = false;//隐藏Icon显示
            doc.ToolTip = path;

            doc.MakeView();

            //工作流运行或调试时新打开的xaml文件要置为只读
            if (_workflowStateService.IsRunningOrDebugging)
            {
                doc.IsReadOnly = true;
            }

            Documents.Add(doc);

            doc.IsSelected = true;//由于IsSelected会触发事件，所以要放到MakeView()后触发


            return doc;
        }


        public bool CloseAllQuery()
        {
            //首先遍历所有文档，依次调用关闭TAB页文档功能（TAB页关闭命令有修改的提示用户是否要保存文档）
            var docList = Documents.ToList();//先转成list再遍历，不然关闭时会修改Documents集合导致异常
            foreach (var doc in docList)
            {
                if (!doc.CloseQuery())
                {
                    return false;//只要点取消了就不往下走了
                }
            }

            return true;
        }


        /// <summary>
        /// 如果doc对应的Path不存在，则直接关闭该文档
        /// </summary>
        public void OnDeleteFile(string path)
        {
            var docList = Documents.ToList();
            foreach (var doc in docList)
            {
                //取巧的办法，对应路径不存在则关闭文档
                if (!System.IO.File.Exists(doc.Path))
                {
                    Documents.Remove(doc);
                }
            }
        }


        public void OnDeleteDir(string path)
        {
            var docList = Documents.ToList();
            foreach (var doc in docList)
            {
                //取巧的办法，对应路径不存在则关闭文档
                if (!System.IO.File.Exists(doc.Path))
                {
                    Documents.Remove(doc);
                }
            }
        }


        public bool IsDocumentExist<T>(string path, out T retDoc) where T : DocumentViewModel
        {
            foreach (var doc in Documents)
            {
                if (doc is T)
                {
                    if (doc.Path == path)
                    {
                        retDoc = (T)doc;
                        return true;
                    }
                }
            }

            retDoc = null;
            return false;
        }

        /// <summary>
        /// 检查已打开的文档，将路径修正
        /// </summary>
        /// <param name="renameViewModel"></param>
        public void OnRename(RenameViewModel renameViewModel)
        {
            var docList = Documents.ToList();
            foreach (var doc in docList)
            {
                if (renameViewModel.IsDirectory)
                {
                    if (doc.Path.ContainsIgnoreCase(renameViewModel.Path))
                    {
                        doc.Path = doc.Path.Replace(renameViewModel.Path + @"\", renameViewModel.NewPath + @"\");
                        doc.ToolTip = doc.Path;
                        doc.UpdatePathCrossDomain(doc.Path);
                    }
                }
                else
                {
                    if (doc.Path.EqualsIgnoreCase(renameViewModel.Path))
                    {
                        doc.Path = doc.Path.Replace(renameViewModel.Path, renameViewModel.NewPath);
                        doc.ToolTip = doc.Path;
                        doc.Title = System.IO.Path.GetFileNameWithoutExtension(renameViewModel.NewPath);
                        doc.UpdatePathCrossDomain(doc.Path);
                    }
                }
            }
        }



    }
}