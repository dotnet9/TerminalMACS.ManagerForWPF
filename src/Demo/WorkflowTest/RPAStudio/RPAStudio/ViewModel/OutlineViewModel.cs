using GalaSoft.MvvmLight;
using System.Windows;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OutlineViewModel : ViewModelBase
    {
        private DocksViewModel _docksViewModel;

        /// <summary>
        /// Initializes a new instance of the OutlineViewModel class.
        /// </summary>
        public OutlineViewModel(DocksViewModel docksViewModel)
        {
            _docksViewModel = docksViewModel;

            _docksViewModel.DocumentSelectChangeEvent += _documentsViewModel_DocumentSelectChangeEvent;
        }


        private void _documentsViewModel_DocumentSelectChangeEvent(object sender, System.EventArgs e)
        {
            if (sender is DesignerDocumentViewModel)
            {
                var designerDoc = sender as DesignerDocumentViewModel;
                WorkflowOutlineView = designerDoc.WorkflowOutlineView;
            }
            else
            {
                WorkflowOutlineView = null;
            }
        }


        /// <summary>
        /// The <see cref="WorkflowOutlineView" /> property's name.
        /// </summary>
        public const string WorkflowOutlineViewPropertyName = "WorkflowOutlineView";

        private FrameworkElement _workflowOutlineViewProperty = null;

        /// <summary>
        /// Sets and gets the WorkflowOutlineView property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public FrameworkElement WorkflowOutlineView
        {
            get
            {
                return _workflowOutlineViewProperty;
            }

            set
            {
                if (_workflowOutlineViewProperty == value)
                {
                    return;
                }

                _workflowOutlineViewProperty = value;
                RaisePropertyChanged(WorkflowOutlineViewPropertyName);
            }
        }




    }
}