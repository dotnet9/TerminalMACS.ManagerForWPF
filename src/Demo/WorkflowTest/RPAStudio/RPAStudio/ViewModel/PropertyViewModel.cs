using GalaSoft.MvvmLight;
using RPA.Interfaces.Share;
using System.Windows;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PropertyViewModel : ViewModelBase
    {
        private DocksViewModel _docksViewModel;

        /// <summary>
        /// Initializes a new instance of the PropertyViewModel class.
        /// </summary>
        public PropertyViewModel(DocksViewModel docksViewModel)
        {
            _docksViewModel = docksViewModel;

            _docksViewModel.DocumentSelectChangeEvent += _documentsViewModel_DocumentSelectChangeEvent;
        }

        private void _documentsViewModel_DocumentSelectChangeEvent(object sender, System.EventArgs e)
        {
            if (sender is DesignerDocumentViewModel)
            {
                var designerDoc = sender as DesignerDocumentViewModel;
                WorkflowPropertyView = designerDoc.WorkflowPropertyView;

                designerDoc.GetWorkflowDesignerServiceProxy().UpdateCurrentSelecteddDesigner();
            }
            else
            {
                WorkflowPropertyView = null;
            }
        }


        /// <summary>
        /// The <see cref="WorkflowPropertyView" /> property's name.
        /// </summary>
        public const string WorkflowPropertyViewPropertyName = "WorkflowPropertyView";

        private FrameworkElement _workflowPropertyViewProperty = null;

        /// <summary>
        /// 工作流属性视图
        /// </summary>
        public FrameworkElement WorkflowPropertyView
        {
            get
            {
                return _workflowPropertyViewProperty;
            }

            set
            {
                if (_workflowPropertyViewProperty == value)
                {
                    return;
                }

                _workflowPropertyViewProperty = value;
                RaisePropertyChanged(WorkflowPropertyViewPropertyName);
            }
        }


    }
}