using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace RAPTest2;

public partial class MainWindow : Window
{
    private WorkflowDesigner? _workflowDesigner; // 工作流设计器
    private string? _savePath;

    private enum DesignerType
    {
        Sequence,
        Flowchart,
        StateMachine
    }

    public MainWindow()
    {
        InitializeComponent();
        (new DesignerMetadata()).Register(); // 注册元数据
        this.NewDesigner(DesignerType.Sequence); // 默认创建序列图
    }

    private void UpdateView()
    {
        this.DesignerBorder.Child = this._workflowDesigner!.View; // 设计器视图
        this.PropertyBorder.Child = this._workflowDesigner.PropertyInspectorView; // 属性面板
        this.OutlineBorder.Child = this._workflowDesigner.OutlineView; // 大纲面板
    }

    private void NewDesigner(DesignerType type)
    {
        this._workflowDesigner = new WorkflowDesigner();

        switch (type)
        {
            case DesignerType.Sequence:
                this._workflowDesigner.Load(new ActivityBuilder() { Implementation = new Sequence(), Name = "Main" });
                break;
            case DesignerType.Flowchart:
                this._workflowDesigner.Load(new ActivityBuilder() { Implementation = new Flowchart(), Name = "Main" });
                break;
            case DesignerType.StateMachine:
                this._workflowDesigner.Load(
                    new ActivityBuilder() { Implementation = new StateMachine(), Name = "Main" });
                break;
            default:
                break;
        }

        this.UpdateView();
    }

    private void Sequence_OnClick(object sender, RoutedEventArgs e)
    {
        this.NewDesigner(DesignerType.Sequence);
    }

    private void Flowchart_OnClick(object sender, RoutedEventArgs e)
    {
        this.NewDesigner(DesignerType.Flowchart);
    }

    private void StateMachine_OnClick(object sender, RoutedEventArgs e)
    {
        this.NewDesigner(DesignerType.StateMachine);
    }

    private void Open_OnClick(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog();
        dlg.Filter = "XAML 文件(*.xaml)|*.xaml";
        dlg.RestoreDirectory = true;
        if (dlg.ShowDialog() == true)
        {
            var openPath = dlg.FileName;
            this._savePath = openPath;
            this._workflowDesigner = new WorkflowDesigner();
            this._workflowDesigner.Load(openPath);
            UpdateView();
        }
    }

    private void SaveCurrentDesigner()
    {
        if (string.IsNullOrWhiteSpace(this._savePath))
        {
            this.SaveCurrentDesignerAs();
        }
        else
        {
            this.SaveXAML();
        }
    }

    private void SaveXAML()
    {
        this._workflowDesigner!.Flush();
        var xamlText = this._workflowDesigner.Text;
        File.WriteAllText(this._savePath!, xamlText);
    }

    private void SaveCurrentDesignerAs()
    {
        var dlg = new SaveFileDialog();
        dlg.FileName = "*.xaml";
        dlg.Filter = "XAML文件 （*.xaml)|*.xaml";
        dlg.RestoreDirectory = true;

        if (dlg.ShowDialog() == true)
        {
            this._savePath = dlg.FileName;
            this.SaveXAML();
        }
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        this.SaveCurrentDesigner();
    }

    private void SaveAs_OnClick(object sender, RoutedEventArgs e)
    {
        this.SaveCurrentDesignerAs();
    }

    public void OutputLine(string msg)
    {
        OutputTextBox.Dispatcher.Invoke(() =>
        {
            OutputTextBox.Text += msg;
            OutputTextBox.Text += Environment.NewLine;
        });
    }

    private void Run_OnClick(object sender, RoutedEventArgs e)
    {
        OutputTextBox.Clear();

        this._workflowDesigner!.Flush();
        var workflowStream = new MemoryStream(Encoding.UTF8.GetBytes(this._workflowDesigner.Text));
        var workflow = ActivityXamlServices.Load(workflowStream);

        var result = ActivityValidationServices.Validate(workflow); // 校验工作流
        if (result.Errors.Count == 0)
        {
            OutputLine("工作流执行开始");

            var inputs = new Dictionary<string, object>();
            inputs["argument1"] = "Hello World!";
            var app = new WorkflowApplication(workflow,inputs)
            {
                OnUnhandledException = this.WorkflowApplicationOnUnhandledException,
                Completed = this.WorkflowApplicationExecutionComplete,
            };
            app.Extensions.Add(new RedirectToOutputTextWriter(this));
            app.Run();
        }
        else
        {
            foreach (var  err in result.Errors)
            {
                OutputLine(err.Message);
            }
        }
    }

    private UnhandledExceptionAction WorkflowApplicationOnUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs args)
    {
        var expStr = $"{args.UnhandledException.ToString()}";

        OutputLine(expStr);
        return UnhandledExceptionAction.Terminate;
    }

    private void WorkflowApplicationExecutionComplete(WorkflowApplicationCompletedEventArgs args)
    {
        OutputLine("工作流执行结束");
    }
}