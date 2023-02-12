using Activities.Shared.Converters;
using Microsoft.VisualBasic.Activities;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RPA.Learn.Activities.Workflow
{
    // InvokeWorkflowFileDesigner.xaml 的交互逻辑
    public partial class InvokeWorkflowFileDesigner
    {
        public InvokeWorkflowFileDesigner()
        {
            InitializeComponent();
        }

        private void EditArgumentsBtn_Click(object sender, RoutedEventArgs e)
        {
            ModelItem mi = this.ModelItem.Properties["Arguments"].Dictionary;
            var options = new DynamicArgumentDesignerOptions()
            {
                Title = "编辑工作流参数"
            };

            DynamicArgumentDialog.ShowDialog(this.ModelItem, mi, Context, this.ModelItem.View, options);
        }

        private void ImportArgumentsBtn_Click(object sender, RoutedEventArgs e)
        {
            var workflowFilePathArg = ModelItem.Properties["WorkflowFilePath"].ComputedValue as InArgument<string>;

            var workflowFilePath = "";
            if (workflowFilePathArg != null)
            {
                workflowFilePath = new VariableToTextConverter().Convert(workflowFilePathArg, typeof(string), null, CultureInfo.CurrentCulture) as string;

                if (!System.IO.Path.IsPathRooted(workflowFilePath))
                {
                    workflowFilePath = System.IO.Path.Combine(SharedObject.Instance.ProjectPath, workflowFilePath);
                }
            }

            Dictionary<string, Argument> argDict = new Dictionary<string, Argument>();

            if (!string.IsNullOrEmpty(workflowFilePath))
            {
                try
                {
                    var activity = ActivityXamlServices.Load(workflowFilePath) as DynamicActivity;
                    foreach (var prop in activity.Properties)
                    {
                        if (!argDict.ContainsKey(prop.Name))
                        {
                            if (prop.Value == null)
                            {
                                argDict.Add(prop.Name, (Argument)Activator.CreateInstance(prop.Type));
                            }
                            else
                            {
                                argDict.Add(prop.Name, (Argument)prop.Value);
                            }

                        }
                    }
                }
                catch (Exception)
                {
                   
                }

            }

            var options = new DynamicArgumentDesignerOptions()
            {
                Title = "导入工作流参数"
            };

            ModelTreeManager mtm = new ModelTreeManager(new EditingContext());
            mtm.Load(argDict);

            if (DynamicArgumentDialog.ShowDialog(this.ModelItem, mtm.Root, Context, this.ModelItem.View, options))
            {
                var saveArgDict = this.ModelItem.Properties["Arguments"].Dictionary;
                saveArgDict.Clear();
                foreach (var item in argDict)
                {
                    saveArgDict.Add(item.Key, item.Value);
                }
            }
        }

        private void BrowserBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = $"工作流文件 (*{ProjectConstantConfig.XamlFileExtension})|*{ProjectConstantConfig.XamlFileExtension}|所有文件 (*.*)|*.*";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = SharedObject.Instance.ProjectPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (filePath.StartsWith(SharedObject.Instance.ProjectPath, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    //如果在项目目录下，则使用相对路径保存
                    filePath = Common.MakeRelativePath(SharedObject.Instance.ProjectPath, filePath);
                }

                ModelItem.Properties["WorkflowFilePath"].SetValue(new InArgument<string>(filePath));
            }
        }


    }
}
