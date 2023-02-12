using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Workflow
{
    /// <summary>
    /// 速度类型
    /// </summary>
    public enum enSpeed
    {
        Off,//关闭
        One,//1x
        Two,//2x
        Three,//3x
        Four//4x
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum enOperate
    {
        Null,//无操作
        StepInto,//步入
        StepOver,//步过
        Continue,//继续
        Break,//中断
        Stop,//停止
    }

    public interface IWorkflowDebugService
    {
        void Init(IWorkflowDesignerServiceProxy workflowDesignerServiceProxy, string xamlPath, List<string> activitiesDllLoadFrom, List<string> dependentAssemblies);

        void Debug();
        void Stop();

        void SetSpeed(enSpeed speedType);
        void Break();
        void Continue(enOperate operate = enOperate.Continue);
        void SetNextOperate(enOperate operate);
    }
}
