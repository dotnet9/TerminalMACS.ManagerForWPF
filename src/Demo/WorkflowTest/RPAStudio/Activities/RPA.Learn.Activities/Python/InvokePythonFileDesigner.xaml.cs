using RPA.Interfaces.Share;
using RPA.Shared.Utils;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
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

namespace RPA.Learn.Activities.Python
{
    // InvokePythonFileDesigner.xaml 的交互逻辑
    public partial class InvokePythonFileDesigner
    {
        public InvokePythonFileDesigner()
        {
            InitializeComponent();
        }

        private void BrowserBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Python 脚本文件 (*.py)|*.py";
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

                ModelItem.Properties["PythonFilePath"].SetValue(new InArgument<string>(filePath));
            }
        }


        private void ChangeWorkingDirectoryBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择Python脚本执行时的工作目录";
            dialog.SelectedPath = SharedObject.Instance.ProjectPath;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ModelItem.Properties["PythonWorkingDirectory"].SetValue(new InArgument<string>(dialog.SelectedPath));
            }
        }

        private void EditArgumentsBtn_Click(object sender, RoutedEventArgs e)
        {
            DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions
            {
                Title = "执行Python脚本文件参数设置"
            };
            ModelItemDictionary dictionary = base.ModelItem.Properties["Arguments"].Dictionary;
            using (ModelEditingScope modelEditingScope = dictionary.BeginEdit("PythonScriptFileArgumentEditing"))
            {
                if (DynamicArgumentDialog.ShowDialog(base.ModelItem, dictionary, base.Context, base.ModelItem.View, options))
                {
                    modelEditingScope.Complete();
                }
                else
                {
                    modelEditingScope.Revert();
                }
            }
        }


    }
}
