using GongSolutions.Wpf.DragDrop;
using RPAStudio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPAStudio.DragDrop
{
    public class SnippetItemDragHandler : DefaultDragHandler
    {
        public override bool CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo.SourceItem is SnippetItemViewModel)
            {
                return true;
            }
            return false;
        }


        public override void StartDrag(IDragInfo dragInfo)
        {
            var item = dragInfo.SourceItem as SnippetItemViewModel;
            DataObject data = new DataObject(System.Activities.Presentation.DragDropHelper.WorkflowItemTypeNameFormat, item.ActivityFactoryAssemblyQualifiedName);
            dragInfo.DataObject = data;
            data.SetData("FilePath", item.Path);
            data.SetData("DisplayName", item.Name);
            data.SetData("FileType", "Snippet");
            base.StartDrag(dragInfo);
        }
    }
}
