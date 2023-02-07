using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace RAPTest2;

public partial class MainWindow : Window
{
    private string? _savePath;
    private WorkflowDesigner? _workflowDesigner; // 工作流设计器

    public MainWindow()
    {
        InitializeComponent();
        new DesignerMetadata().Register(); // 注册元数据
        NewDesigner(DesignerType.Sequence); // 默认创建序列图
    }

    private void UpdateView()
    {
        DesignerBorder.Child = _workflowDesigner!.View; // 设计器视图
        PropertyBorder.Child = _workflowDesigner.PropertyInspectorView; // 属性面板
        OutlineBorder.Child = _workflowDesigner.OutlineView; // 大纲面板
    }

    private void NewDesigner(DesignerType type)
    {
        _workflowDesigner = new WorkflowDesigner();

        switch (type)
        {
            case DesignerType.Sequence:
                _workflowDesigner.Load(new ActivityBuilder { Implementation = new Sequence(), Name = "Main" });
                break;
            case DesignerType.Flowchart:
                _workflowDesigner.Load(new ActivityBuilder { Implementation = new Flowchart(), Name = "Main" });
                break;
            case DesignerType.StateMachine:
                _workflowDesigner.Load(
                    new ActivityBuilder { Implementation = new StateMachine(), Name = "Main" });
                break;
        }

        UpdateView();
    }

    private void Sequence_OnClick(object sender, RoutedEventArgs e)
    {
        NewDesigner(DesignerType.Sequence);
    }

    private void Flowchart_OnClick(object sender, RoutedEventArgs e)
    {
        NewDesigner(DesignerType.Flowchart);
    }

    private void StateMachine_OnClick(object sender, RoutedEventArgs e)
    {
        NewDesigner(DesignerType.StateMachine);
    }

    private void Open_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dlg = new();
        dlg.Filter = "XAML 文件(*.xaml)|*.xaml";
        dlg.RestoreDirectory = true;
        if (dlg.ShowDialog() == true)
        {
            string openPath = dlg.FileName;
            _savePath = openPath;
            _workflowDesigner = new WorkflowDesigner();
            _workflowDesigner.Load(openPath);
            UpdateView();
        }
    }

    private void SaveCurrentDesigner()
    {
        if (string.IsNullOrWhiteSpace(_savePath))
        {
            SaveCurrentDesignerAs();
        }
        else
        {
            SaveXAML();
        }
    }

    private void SaveXAML()
    {
        _workflowDesigner!.Flush();
        string? xamlText = _workflowDesigner.Text;
        File.WriteAllText(_savePath!, xamlText);
    }

    private void SaveCurrentDesignerAs()
    {
        SaveFileDialog dlg = new();
        dlg.FileName = "*.xaml";
        dlg.Filter = "XAML文件 （*.xaml)|*.xaml";
        dlg.RestoreDirectory = true;

        if (dlg.ShowDialog() == true)
        {
            _savePath = dlg.FileName;
            SaveXAML();
        }
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        SaveCurrentDesigner();
    }

    private void SaveAs_OnClick(object sender, RoutedEventArgs e)
    {
        SaveCurrentDesignerAs();
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

        _workflowDesigner!.Flush();
        MemoryStream workflowStream = new(Encoding.UTF8.GetBytes(_workflowDesigner.Text));
        Activity workflow = ActivityXamlServices.Load(workflowStream);

        ValidationResults result = ActivityValidationServices.Validate(workflow); // 校验工作流
        if (result.Errors.Count == 0)
        {
            OutputLine("工作流执行开始");

            Dictionary<string, object> inputs = new();
            inputs["argument1"] = "Hello World!";
            WorkflowApplication app = new(workflow, inputs)
            {
                OnUnhandledException = WorkflowApplicationOnUnhandledException,
                Completed = WorkflowApplicationExecutionComplete
            };
            app.Extensions.Add(new RedirectToOutputTextWriter(this));
            app.Run();
        }
        else
        {
            foreach (ValidationError? err in result.Errors)
            {
                OutputLine(err.Message);
            }
        }
    }

    private UnhandledExceptionAction WorkflowApplicationOnUnhandledException(
        WorkflowApplicationUnhandledExceptionEventArgs args)
    {
        string expStr = $"{args.UnhandledException}";

        OutputLine(expStr);
        return UnhandledExceptionAction.Terminate;
    }

    private void WorkflowApplicationExecutionComplete(WorkflowApplicationCompletedEventArgs args)
    {
        OutputLine("工作流执行结束");
    }

    private enum DesignerType
    {
        Sequence,
        Flowchart,
        StateMachine
    }
}