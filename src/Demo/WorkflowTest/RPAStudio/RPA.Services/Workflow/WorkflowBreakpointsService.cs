using Newtonsoft.Json.Linq;
using NLog;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Project;
using RPA.Interfaces.Workflow;
using RPA.Shared.Configs;
using RPA.Shared.Debugger;
using RPA.Shared.Utils;
using System;
using System.Activities;
using System.Activities.Debugger;
using System.Activities.Presentation;
using System.Activities.Presentation.Debug;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Workflow
{
    public class WorkflowBreakpointsService : MarshalByRefServiceBase, IWorkflowBreakpointsService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IWorkflowDesignerCollectService _workflowDesignerCollectService;
        private IProjectManagerService _projectManagerService;

        private Dictionary<string, JArray> _breakpointsDict = new Dictionary<string, JArray>();

        public WorkflowBreakpointsService(IWorkflowDesignerCollectService workflowDesignerCollectService
            , IProjectManagerService projectManagerService
            )
        {
            _workflowDesignerCollectService = workflowDesignerCollectService;
            _projectManagerService = projectManagerService;
        }


        public void SaveBreakpoints()
        {
            var projectPath = _projectManagerService.CurrentProjectPath;

            if (!string.IsNullOrEmpty(projectPath))
            {
                if (_breakpointsDict != null)
                {
                    //保存项目配置
                    JObject rootJsonObj = new JObject();
                    JObject projectBreakpointsjsonObj = new JObject();
                    rootJsonObj["ProjectBreakpoints"] = projectBreakpointsjsonObj;

                    JObject valueJsonObj = new JObject();
                    projectBreakpointsjsonObj["Value"] = valueJsonObj;

                    foreach (var item in _breakpointsDict)
                    {
                        JArray jarrayJsonObj = new JArray();
                        valueJsonObj[item.Key] = jarrayJsonObj;

                        foreach (JToken ji in item.Value)
                        {
                            JObject itemJsonObj = new JObject();
                            itemJsonObj["ActivityId"] = (ji as JObject)["ActivityId"].ToString();
                            itemJsonObj["IsEnabled"] = (bool)(ji as JObject)["IsEnabled"];
                            jarrayJsonObj.Add(itemJsonObj);
                        }

                    }

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(rootJsonObj, Newtonsoft.Json.Formatting.Indented);
                    var projectSettingsjson = projectPath + $"\\{ProjectConstantConfig.ProjectLocalDirectoryName}\\{ProjectConstantConfig.ProjectSettingsFileName}";

                    //创建.local隐藏目录
                    var localDir = projectPath + @"\" + ProjectConstantConfig.ProjectLocalDirectoryName;
                    if (!System.IO.Directory.Exists(localDir))
                    {
                        System.IO.Directory.CreateDirectory(localDir);
                        System.IO.File.SetAttributes(localDir, System.IO.FileAttributes.Hidden);
                    }

                    System.IO.File.WriteAllText(projectSettingsjson, output);
                }

            }
        }

        public void LoadBreakpoints()
        {
            //加载项目配置
            var projectSettingsjson = _projectManagerService.CurrentProjectPath + $"\\{ProjectConstantConfig.ProjectLocalDirectoryName}\\{ProjectConstantConfig.ProjectSettingsFileName}";
            if (System.IO.File.Exists(projectSettingsjson))
            {
                string json = System.IO.File.ReadAllText(projectSettingsjson);
                JObject rootJsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;

                var valueJsonObj = rootJsonObj["ProjectBreakpoints"]["Value"];
                if (valueJsonObj != null)
                {
                    foreach (JProperty jp in valueJsonObj)
                    {
                        _breakpointsDict[jp.Name] = (JArray)jp.Value;
                    }
                }
            }
        }

        public void ShowBreakpoints(string path)
        {
            WorkflowDesigner workflowDesigner = _workflowDesignerCollectService.Get(path)?.GetWorkflowDesigner();
            var relativeXamlPath = Common.MakeRelativePath(_projectManagerService.CurrentProjectPath, path);

            var breakpointsDict = _breakpointsDict;
            if (breakpointsDict.Count > 0)
            {
                if (breakpointsDict.ContainsKey(relativeXamlPath))
                {
                    JArray jarr = (JArray)breakpointsDict[relativeXamlPath].DeepClone();

                    foreach (JToken ji in jarr)
                    {
                        var activityId = ((JObject)ji)["ActivityId"].ToString();
                        var IsEnabled = (bool)(((JObject)ji)["IsEnabled"]);

                        SetBreakpoint(path, relativeXamlPath, activityId, IsEnabled);
                    }
                }
            }
        }

        public void RemoveAllBreakpoints(string path)
        {
            WorkflowDesigner workflowDesigner = _workflowDesignerCollectService.Get(path)?.GetWorkflowDesigner();

            workflowDesigner.DebugManagerView.ResetBreakpoints();

            _breakpointsDict.Clear();
        }

        private void SetBreakpoint(string path, string relativeXamlPath, string activityId, bool IsEnabled)
        {
            WorkflowDesigner workflowDesigner = _workflowDesignerCollectService.Get(path)?.GetWorkflowDesigner();

            try
            {
                Dictionary<string, SourceLocation> activityIdToSourceLocationMapping = new Dictionary<string, SourceLocation>();
                RPADebugger.BuildSourceLocationMappings(workflowDesigner, ref activityIdToSourceLocationMapping);

                if (activityIdToSourceLocationMapping.ContainsKey(activityId))
                {
                    SourceLocation srcLoc = activityIdToSourceLocationMapping[activityId];

                    if (IsEnabled)
                    {
                        workflowDesigner.DebugManagerView.InsertBreakpoint(srcLoc, BreakpointTypes.Enabled | BreakpointTypes.Bounded);
                    }
                    else
                    {
                        workflowDesigner.DebugManagerView.DeleteBreakpoint(srcLoc);
                    }
                }
                else
                {
                    //找不到断点位置，说明文件有修改，则该断点信息删除
                    RemoveBreakpointLocation(relativeXamlPath, activityId);
                }
            }
            catch (Exception err)
            {
                _logger.Debug(err);
            }

        }

        private void RemoveBreakpointLocation(string relativeXamlPath, string activityId)
        {
            if (_breakpointsDict.ContainsKey(relativeXamlPath))
            {
                var jarr = _breakpointsDict[relativeXamlPath];

                foreach (JToken ji in jarr)
                {
                    if (((JObject)ji)["ActivityId"].ToString() == activityId)
                    {
                        ji.Remove();
                        break;
                    }
                }
            }
        }

        public void ToggleBreakpoint(string path)
        {
            WorkflowDesigner workflowDesigner = _workflowDesignerCollectService.Get(path)?.GetWorkflowDesigner();
            var relativeXamlPath = Common.MakeRelativePath(_projectManagerService.CurrentProjectPath, path);

            try
            {
                Activity activity = workflowDesigner.Context.Items.GetValue<Selection>().
                        PrimarySelection.GetCurrentValue() as Activity;

                if (activity != null)
                {
                    //切换该活动的断点
                    Dictionary<string, SourceLocation> activityIdToSourceLocationMapping = new Dictionary<string, SourceLocation>();
                    RPADebugger.BuildSourceLocationMappings(workflowDesigner, ref activityIdToSourceLocationMapping);

                    if (!activityIdToSourceLocationMapping.ContainsKey(activity.Id))
                    {
                        return;
                    }
                    SourceLocation srcLoc = activityIdToSourceLocationMapping[activity.Id];

                    bool bInsertBreakpoint = false;
                    var breakpointLocations = workflowDesigner.DebugManagerView.GetBreakpointLocations();
                    if (breakpointLocations.ContainsKey(srcLoc))
                    {
                        var types = breakpointLocations[srcLoc];
                        if (types != (BreakpointTypes.Enabled | BreakpointTypes.Bounded))
                        {
                            bInsertBreakpoint = true;
                        }
                    }
                    else
                    {
                        bInsertBreakpoint = true;
                    }

                    if (bInsertBreakpoint)
                    {
                        workflowDesigner.DebugManagerView.InsertBreakpoint(srcLoc, BreakpointTypes.Enabled | BreakpointTypes.Bounded);
                        AddBreakpointLocation(relativeXamlPath, activity.Id, true);
                    }
                    else
                    {
                        workflowDesigner.DebugManagerView.DeleteBreakpoint(srcLoc);
                        RemoveBreakpointLocation(relativeXamlPath, activity.Id);
                    }

                }
            }
            catch (Exception err)
            {
                //特殊情况时触发，目前在flowchart中如果不连接start node,也会报错
                _logger.Debug(err);
            }
        }

        private void AddBreakpointLocation(string relativeXamlPath, string activityId, bool isEnabled)
        {
            RemoveBreakpointLocation(relativeXamlPath, activityId);

            if (!_breakpointsDict.ContainsKey(relativeXamlPath))
            {
                _breakpointsDict.Add(relativeXamlPath, new JArray());
            }

            var jarr = _breakpointsDict[relativeXamlPath];
            JObject jobj = new JObject();
            jobj["ActivityId"] = activityId;
            jobj["IsEnabled"] = isEnabled;
            jarr.Add(jobj);
        }




    }
}
