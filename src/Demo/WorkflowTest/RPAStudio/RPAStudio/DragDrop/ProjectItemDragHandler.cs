using RPAStudio.ViewModel;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Activities.Shared.ActivityTemplateFactory;

namespace RPAStudio.DragDrop
{
    public class ProjectItemDragHandler : DefaultDragHandler
    {
        public override bool CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo.SourceItem is ProjectDirItemViewModel)
            {
                return true;
            }
            else if (dragInfo.SourceItem is ProjectFileItemViewModel)
            {
                var item = dragInfo.SourceItem as ProjectFileItemViewModel;
                if(item.IsXamlFile)
                {
                    return true;
                }

                if(item.IsPythonFile)
                {
                    return true;
                }

                if (item.IsJavaScriptFile)
                {
                    return true;
                }
            }

            return false;
        }


        public override void StartDrag(IDragInfo dragInfo)
        {
            if (dragInfo.SourceItem is ProjectDirItemViewModel)
            {
                var item = dragInfo.SourceItem as ProjectDirItemViewModel;
            }
            else if (dragInfo.SourceItem is ProjectFileItemViewModel)
            {
                var item = dragInfo.SourceItem as ProjectFileItemViewModel;

                DataObject data = new DataObject(System.Activities.Presentation.DragDropHelper.WorkflowItemTypeNameFormat, item.ActivityFactoryAssemblyQualifiedName);
                data.SetData("FilePath", item.Path);

                if (item.IsXamlFile)
                {
                    data.SetData("FileType", "Xaml");
                    data.SetData("TypeOf", InvokeWorkflowFileFactory.ActivityTypeName);
                }
                else if (item.IsPythonFile)
                {
                    data.SetData("FileType", "Python");
                    data.SetData("TypeOf", InvokePythonFileFactory.ActivityTypeName);
                }
                else
                {

                }

                data.SetData("DisplayName", item.ActivityDisplayName);
                data.SetData("AssemblyQualifiedName", item.ActivityAssemblyQualifiedName);
                dragInfo.DataObject = data;
            }

            base.StartDrag(dragInfo);
        }
    }
}
