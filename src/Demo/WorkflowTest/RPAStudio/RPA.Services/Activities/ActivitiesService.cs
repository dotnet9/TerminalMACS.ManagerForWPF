using NLog;
using RPA.Interfaces.Activities;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
using System;
using System.Activities;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Services.Activities
{
    public class ActivitiesService : MarshalByRefServiceBase, IActivitiesService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public List<string> Assemblies { get; private set; } = new List<string>();

        private IAssemblyResolveService _assemblyResolveService;

        public List<string> CustomActivityConfigXmls { get; private set; } = new List<string>();

        public ActivitiesService(IAssemblyResolveService assemblyResolveService)
        {
            _assemblyResolveService = assemblyResolveService;
        }

        public List<string> Init(List<string> assemblies)
        {
            Assemblies = assemblies;

            _assemblyResolveService.Init(Assemblies);

            CustomActivityConfigXmls.Clear();

            var activitiesDllList = Assemblies.Where(item => Regex.IsMatch(item, ProjectConstantConfig.ProjectActivitiesAssemblyMatchRegex, RegexOptions.IgnoreCase)).ToList();

            foreach (var dllFile in activitiesDllList)
            {
                try
                {
                    //判断dll_file对应的文件是否在主程序所在目录存在，若存在，则加载主程序所在目录下的DLL，避免加载不同路径下的相同DLL文件
                    var checkPath = Path.Combine(SharedObject.Instance.ApplicationCurrentDirectory, Path.GetFileName(dllFile));
                    if (System.IO.File.Exists(checkPath))
                    {
                        _logger.Debug($"忽略加载 {dllFile}");
                        continue;//避免加载重复的DLL
                    }

                    var asm = Assembly.LoadFrom(dllFile);
                    var dllFileNameWithoutExt = Path.GetFileNameWithoutExtension(dllFile);

                    try
                    {
                        var activitiesXml = Common.GetResourceContentByUri($"pack://application:,,,/{dllFileNameWithoutExt};Component/Activities.xml");
                        CustomActivityConfigXmls.Add(activitiesXml);
                    }
                    catch (Exception err)
                    {
                        SharedObject.Instance.Output(SharedObject.enOutputType.Error, $"挂载{dllFile}中的Activities.xml信息出错", err);
                    }

                }
                catch (Exception err)
                {

                }
            }

            return activitiesDllList;
        }

        public Stream GetIcon(string assemblyName, string relativeResPath)
        {

            var stream = Application.GetResourceStream(new Uri($"pack://application:,,,/{assemblyName};Component/{relativeResPath}", UriKind.Absolute)).Stream;

            return stream;
        }

        public string GetAssemblyQualifiedName(string typeOf)
        {
            try
            {
                Type type = null;

                if (typeOf.Contains(","))
                {
                    type = Type.GetType(typeOf);//此处借助AssemblyResolveService进行类型获取，否则可能会返回null

                    if (type == null)
                    {
                        //可能有泛型，添加`x再尝试下
                        string[] sArray = typeOf.Split(',');
                        if (sArray.Length > 1)
                        {
                            sArray[0] += "`1[[System.Object,mscorlib,Version=4.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089]]";
                        }
                        type = Type.GetType(string.Join(",", sArray));
                    }

                    if (type == null)
                    {
                        //可能有泛型，添加`x再尝试下
                        string[] sArray = typeOf.Split(',');
                        if (sArray.Length > 1)
                        {
                            sArray[0] += "`2[[System.Object,mscorlib,Version=4.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089]]";
                        }

                        type = Type.GetType(string.Join(",", sArray));
                    }
                }
                else
                {
                    //没有逗号，说明是系统自带的组件

                    if (typeOf == "FinalState")
                    {
                        //特殊处理
                        type = typeof(System.Activities.Core.Presentation.FinalState);
                    }
                    else if (typeOf == "ForEach")
                    {
                        type = typeof(System.Activities.Core.Presentation.Factories.ForEachWithBodyFactory<object>);
                    }
                    else if (typeOf == "ParallelForEach")
                    {
                        type = typeof(System.Activities.Core.Presentation.Factories.ParallelForEachWithBodyFactory<object>);
                    }
                    else if (typeOf == "Switch")
                    {
                        type = typeof(System.Activities.Statements.Switch<string>);
                    }
                    else
                    {
                        type = Type.GetType(string.Format("System.Activities.Statements.{0},System.Activities, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", typeOf));
                        if (type == null)
                        {
                            type = Type.GetType(string.Format("System.Activities.Statements.{0}`1[[System.Object,mscorlib,Version=4.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089]],System.Activities, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", typeOf));
                        }

                        if (type == null)
                        {
                            type = Type.GetType(string.Format("System.Activities.Statements.{0}`2[[System.Object,mscorlib,Version=4.0.0.0,Culture=neutral,PublicKeyToken=b77a5c561934e089]],System.Activities, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", typeOf));
                        }
                    }
                }


                if (type != null)
                {
                    return type.AssemblyQualifiedName;
                }
                else
                {
                    _logger.Error(string.Format("{0} 类型未找到！", typeOf));
                }

            }
            catch (Exception err)
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, err);

            }

            return "";
        }


        public void SetSharedObjectInstance(object instance)
        {
            SharedObject.SetCrossDomainInstance(instance as SharedObject);
        }

        public bool IsXamlValid(string xamlPath)
        {
            Activity workflow = ActivityXamlServices.Load(xamlPath);

            var result = ActivityValidationServices.Validate(workflow);
            if (result.Errors.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool IsXamlStringValid(string xamlString)
        {
            try
            {
                Activity workflow = ActivityXamlServices.Load(new StringReader(xamlString));

                var result = ActivityValidationServices.Validate(workflow);
                if (result.Errors.Count == 0)
                {
                    return true;
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        SharedObject.Instance.Output(SharedObject.enOutputType.Error, err.Message);
                    }

                    return false;
                }
            }
            catch (Exception err)
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, err.Message);
                return false;
            }
        }
    }
}
