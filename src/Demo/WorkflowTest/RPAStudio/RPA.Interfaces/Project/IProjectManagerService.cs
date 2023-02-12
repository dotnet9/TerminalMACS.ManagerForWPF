using RPA.Interfaces.Activities;
using RPA.Shared.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Project
{
    public interface IProjectManagerService
    {
        /// <summary>
        /// 转圈开始
        /// </summary>
        event EventHandler ProjectLoadingBeginEvent;

        /// <summary>
        /// 转圈结束
        /// </summary>
        event EventHandler ProjectLoadingEndEvent;

        /// <summary>
        /// 转圈异常(取消转圈)
        /// </summary>
        event EventHandler ProjectLoadingExceptionEvent;

        /// <summary>
        /// 项目准备打开触发的事件
        /// </summary>
        event EventHandler ProjectPreviewOpenEvent;

        /// <summary>
        /// 项目打开后触发的事件
        /// </summary>
        event EventHandler ProjectOpenEvent;

        /// <summary>
        /// 项目准备关闭触发的事件
        /// </summary>
        event EventHandler<CancelEventArgs> ProjectPreviewCloseEvent;
        /// <summary>
        /// 项目关闭后触发的事件
        /// </summary>
        event EventHandler ProjectCloseEvent;

        /// <summary>
        /// 当前项目的路径
        /// </summary>
        string CurrentProjectPath { get; }


        /// <summary>
        /// 当前项目的main文件对应的绝对路径
        /// </summary>
        string CurrentProjectMainXamlFileAbsolutePath { get; }

        /// <summary>
        /// 当前项目的配置文件路径
        /// </summary>
        string CurrentProjectConfigFilePath { get; }

        /// <summary>
        /// 当前项目的配置
        /// </summary>
        ProjectJsonConfig CurrentProjectJsonConfig { get; }

        /// <summary>
        /// 挂载的活动所有的配置文件字符串
        /// </summary>
        List<string> AllActivityConfigXmls { get; }

        /// <summary>
        /// 挂载后的最终活动树
        /// </summary>
        List<ActivityGroupOrLeafItem> Activities { get; }

        /// <summary>
        /// TypeOf映射
        /// </summary>
        Dictionary<string, ActivityGroupOrLeafItem> ActivitiesTypeOfDict { get; }

        /// <summary>
        /// 获取当前的组件代理，该组件代理每次项目加载后会重新创建
        /// </summary>
        IActivitiesServiceProxy CurrentActivitiesServiceProxy { get; }


        /// <summary>
        /// 当前域加载的依赖项的dll
        /// </summary>
        List<string> CurrentActivitiesDllLoadFrom { get; }

        /// <summary>
        /// 当前域加载的所有依赖的文件
        /// </summary>
        List<string> CurrentDependentAssemblies { get; }

        /// <summary>
        /// 关闭项目,需要询问用户，根据情况返回值
        /// </summary>
        /// <returns>true:关闭成功;false:用户取消关闭或关闭失败</returns>
        bool CloseCurrentProject();


        /// <summary>
        /// 重新打开项目
        /// </summary>
        void ReopenCurrentProject();


        /// <summary>
        /// 新建项目(只新建不打开)
        /// </summary>
        /// <param name="projectsPath">项目目录所在位置</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectDescription">项目描述</param>
        /// <returns>是否创建成功</returns>
        bool NewProject(string projectsPath, string projectName, string projectDescription, string projectVersion);


        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="projectConfigFilePath">项目配置文件路径</param>
        /// <returns>是否打开成功</returns>
        Task OpenProject(string projectConfigFilePath);

        /// <summary>
        /// 是否已经打开项目
        /// </summary>
        /// <param name="projectConfigFilePath">项目配置文件路径</param>
        /// <returns>是否已经打开</returns>
        bool IsAlreadyOpened(string projectConfigFilePath);

        /// <summary>
        /// 保存当前json到文件
        /// </summary>
        /// <returns></returns>
        bool SaveCurrentProjectJson();

        /// <summary>
        /// 更新CurrentProjectConfigFilePath
        /// </summary>
        /// <param name="v"></param>
        void UpdateCurrentProjectConfigFilePath(string projectConfigFilePath);


        /// <summary>
        /// 获取当前项目依赖包的ID对应的版本号信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetCurrentProjectDependencyVersionById(string id);
    }
}