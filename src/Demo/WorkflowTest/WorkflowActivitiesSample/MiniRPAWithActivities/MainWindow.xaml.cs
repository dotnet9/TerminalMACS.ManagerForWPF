using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace MiniRPAWithActivities
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkflowDesigner _wd;//工作流设计器

        private string _savePath;//保存路径

        //设计器类型
        private enum DesignerType
        {
            Sequence,
            Flowchart,
            StateMachine
        }

        public MainWindow()
        {
            InitializeComponent();

            (new DesignerMetadata()).Register();//注册元数据

            NewDesigner(DesignerType.Sequence);//默认创建序列图
        }

        //更新视图
        private void UpdateView()
        {
            DesignerBorder.Child = _wd.View;//设计器视图
            PropertyBorder.Child = _wd.PropertyInspectorView;//属性面板
            OutlineBorder.Child = _wd.OutlineView;//大纲面板
        }

        //输出一行消息到输出窗口
        public void OutputLine(string msg)
        {
            //必须切换到UI线程追加消息
            OutputTextBox.Dispatcher.Invoke(() => {
                OutputTextBox.Text += msg;
                OutputTextBox.Text += Environment.NewLine;//换行
            });
        }

        /// <summary>
        /// 新建设计器面板
        /// </summary>
        /// <param name="dt">指定的设计器类型</param>
        private void NewDesigner(DesignerType dt)
        {
            _wd = new WorkflowDesigner();
            switch (dt)
            {
                case DesignerType.Sequence:
                    _wd.Load(new ActivityBuilder { Implementation = new Sequence(), Name = "Main" });//ActivityBuilder要赋个名字，否则参数设置将无法正常使用
                    break;
                case DesignerType.Flowchart:
                    _wd.Load(new ActivityBuilder { Implementation = new Flowchart(), Name = "Main" });//ActivityBuilder要赋个名字，否则参数设置将无法正常使用
                    break;
                case DesignerType.StateMachine:
                    _wd.Load(new ActivityBuilder { Implementation = new StateMachine(), Name = "Main" });//ActivityBuilder要赋个名字，否则参数设置将无法正常使用
                    break;
                default:
                    break;
            }

            UpdateView();
        }

        //保存XAML文件
        private void SaveXAML()
        {
            _wd.Flush();
            var xamlText = _wd.Text;
            File.WriteAllText(_savePath, xamlText);
        }

        //保存当前设计器内容
        private void SaveCurrentDesigner()
        {
            if (string.IsNullOrEmpty(_savePath))
            {
                SaveCurrentDesignerAs();
            }
            else
            {
                SaveXAML();
            }
        }

        //当前设计器内容另存为
        private void SaveCurrentDesignerAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "*.xaml";
            sfd.Filter = "XAML文件（*.xaml）|*.xaml";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _savePath = sfd.FileName;
                SaveXAML();
            }
        }

        //新建序列图
        private void Sequence_Click(object sender, RoutedEventArgs e)
        {
            NewDesigner(DesignerType.Sequence);
        }

        //新建流程图
        private void Flowchart_Click(object sender, RoutedEventArgs e)
        {
            NewDesigner(DesignerType.Flowchart);
        }
        //新建状态机
        private void StateMachine_Click(object sender, RoutedEventArgs e)
        {
            NewDesigner(DesignerType.StateMachine);
        }

        //打开
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XAML 文件(*.xaml)|*.xaml";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var openPath = ofd.FileName;

                _savePath = openPath;

                _wd = new WorkflowDesigner();
                _wd.Load(openPath);

                UpdateView();
            }
        }

        //保存
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentDesigner();
        }

        //另存为
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentDesignerAs();
        }

        //运行
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            //每次运行前先清空下输出
            OutputTextBox.Clear();

            //运行工作流
            _wd.Flush();
            var workflowStream = new MemoryStream(Encoding.UTF8.GetBytes(_wd.Text));
            var workflow = ActivityXamlServices.Load(workflowStream);

            var result = ActivityValidationServices.Validate(workflow);//校验工作流
            if (result.Errors.Count == 0)
            {
                //校验成功，开始执行工作流
                OutputLine("工作流执行开始");

                IDictionary<string, object> inputs = new Dictionary<string, object>();//Argument参数传值
                //inputs["argument1"] = "Hello World!";
                var app = new WorkflowApplication(workflow, inputs);
                app.OnUnhandledException = WorkflowApplicationOnUnhandledException;
                app.Completed = WorkflowApplicationExecutionCompleted;
                app.Extensions.Add(new RedirectToOutputTextWriter(this));

                app.Run();
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    OutputLine(err.Message);
                }

            }
        }

        private void WorkflowApplicationExecutionCompleted(WorkflowApplicationCompletedEventArgs obj)
        {
            OutputLine("工作流执行结束");
        }

        private UnhandledExceptionAction WorkflowApplicationOnUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs arg)
        {
            var expStr = $"{arg.ExceptionSource.DisplayName} 执行时出现异常：{arg.UnhandledException.ToString()}";

            OutputLine(expStr);
            return UnhandledExceptionAction.Terminate;
        }
    }
}
