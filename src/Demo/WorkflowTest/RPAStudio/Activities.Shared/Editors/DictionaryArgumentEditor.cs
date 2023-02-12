using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Activities.Shared.Editors
{
    /// <summary>
    /// 字典参数编辑器
    /// </summary>
    public class DictionaryArgumentEditor : DialogPropertyValueEditor
    {
        private static DataTemplate EditorTemplate = (DataTemplate)new EditorTemplates()["DictionaryArgumentEditor"];

        public DictionaryArgumentEditor()
        {
            this.InlineEditorTemplate = EditorTemplate;
        }

        //显示对话框
        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            string propertyName = propertyValue.ParentProperty.PropertyName;

            var ownerActivity = (new ModelPropertyEntryToOwnerActivityConverter()).Convert(
                propertyValue.ParentProperty, typeof(ModelItem), false, null) as ModelItem;

            DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions()
            {
                Title = propertyName
            };

            ModelItem modelItem = ownerActivity.Properties[propertyName].Dictionary;

            using (ModelEditingScope change = modelItem.BeginEdit(propertyName + "Editing"))
            {
                if (DynamicArgumentDialog.ShowDialog(ownerActivity, modelItem, ownerActivity.GetEditingContext(), ownerActivity.View, options))
                {
                    change.Complete();
                }
                else
                {
                    change.Revert();
                }
            }
        }
    }
}