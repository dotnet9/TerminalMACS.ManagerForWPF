using RPAStudio.ViewModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace RPAStudio.DragDrop
{
    public class ActivityItemDragHandler : DefaultDragHandler
    {
        public override bool CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo.SourceItem is ActivityLeafItemViewModel)
            {
                return true;
            }
            return false;
        }


        public override void StartDrag(IDragInfo dragInfo)
        {
            var item = dragInfo.SourceItem as ActivityLeafItemViewModel;
            DataObject data = new DataObject(System.Activities.Presentation.DragDropHelper.WorkflowItemTypeNameFormat, item.AssemblyQualifiedName);
            dragInfo.DataObject = data;
            data.SetData("DisplayName", item.Name);
            data.SetData("AssemblyQualifiedName", item.AssemblyQualifiedName);
            data.SetData("TypeOf", item.TypeOf);
            base.StartDrag(dragInfo);
        }
    }
}