using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using Python.Runtime;
using RPA.Interfaces.Share;
using System.Activities.Presentation.Metadata;
using Activities.Shared.Editors;
using System.Activities.Presentation.PropertyEditing;
using RPA.Shared.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RPA.Learn.Activities.Python
{
    [Designer(typeof(InvokePythonFileDesigner))]
    public sealed class InvokePythonFileActivity : CodeActivity
    {
        [Category("输入")]
        [RequiredArgument]
        [Description("Python脚本文件路径，必须用双引号括起来")]
        [DisplayName("Python文件路径")]
        public InArgument<string> PythonFilePath { get; set; }

        [Category("输入")]
        [Description("Python脚本文件执行时的工作目录，默认为当前项目目录")]
        [DisplayName("工作目录")]
        public InArgument<string> PythonWorkingDirectory { get; set; }

        [Category("输入")]
        [Description("导入模块时的搜索路径")]
        [DisplayName("模块搜索路径")]
        public List<InArgument<string>> SysPathAppendList
        {
            get;
            set;
        } = new List<InArgument<string>>();

        [Category("基本")]
        [Description("指定即使当前活动失败，也要继续执行其余的活动。只支持布尔值(True,False)")]
        [DisplayName("错误执行")]
        public InArgument<bool> ContinueOnError
        {
            get;
            set;
        } = false;

        [Category("输入")]
        [Browsable(true)]
        [Description("创建的所有参数的集合")]
        [DisplayName("参数")]
        public Dictionary<string, Argument> Arguments
        {
            get;
            set;
        } = new Dictionary<string, Argument>();

        private static string PythonHome;

        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        private void InitPython()
        {
            if (!PythonEngine.IsInitialized)
            {
                var rpaPythonPath = (Environment.GetEnvironmentVariable("RPA_PYTHON_PATH", EnvironmentVariableTarget.User) ?? Environment.GetEnvironmentVariable("RPA_PYTHON_PATH", EnvironmentVariableTarget.Machine));

                if (System.IO.Directory.Exists(rpaPythonPath))
                {
                    PythonHome = rpaPythonPath;
                }
                else
                {
                    PythonHome = System.IO.Path.Combine(SharedObject.Instance.ApplicationCurrentDirectory, @"Python");
                }

                SharedObject.Instance.Output(SharedObject.enOutputType.Trace, $"使用的PYTHON路径为：“{PythonHome}”");

                Environment.SetEnvironmentVariable("PATH", PythonHome + ";" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine), EnvironmentVariableTarget.Process);

                try
                {
                    PythonEngine.PythonHome = PythonHome;
                    PythonEngine.Initialize();
                }
                catch (Exception err)
                {
                    PythonEngine.Shutdown();
                    SharedObject.Instance.Output(SharedObject.enOutputType.Error, $"初始化Python失败！", err);
                    throw err;
                }
            }
        }

        public InvokePythonFileActivity()
        {
            var builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(typeof(InvokePythonFileActivity), "Arguments", new EditorAttribute(typeof(DictionaryArgumentEditor), typeof(DialogPropertyValueEditor)));
            builder.AddCustomAttributes(typeof(InvokePythonFileActivity), "SysPathAppendList", new EditorAttribute(typeof(ArgumentCollectionEditor), typeof(DialogPropertyValueEditor)));
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            int num = 1;
            foreach (InArgument<string> item in SysPathAppendList)
            {
                RuntimeArgument argument = new RuntimeArgument("sysPathArg" + ++num, typeof(string), ArgumentDirection.In);
                metadata.Bind(item, argument);
                metadata.AddArgument(argument);
            }
        }

        public void SetFilePath(string filePath)
        {
            if (filePath.StartsWith(SharedObject.Instance.ProjectPath, System.StringComparison.CurrentCultureIgnoreCase))
            {
                //如果在项目目录下，则使用相对路径保存
                filePath = Common.MakeRelativePath(SharedObject.Instance.ProjectPath, filePath);
            }

            PythonFilePath = filePath;
        }

        public static object JsonDeserializeArgument(object value, Type argumentType)
        {
            return JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(value)).ToObject(argumentType);
        }

        protected override void Execute(CodeActivityContext context)
        {
            bool flag = ContinueOnError.Get(context);

            IntPtr ts = IntPtr.Zero;

            var prevCurrentDirectory = Environment.CurrentDirectory;

            try
            {
                InitPython();

                ts = PythonEngine.BeginAllowThreads();
                using (Py.GIL())
                {
                    using (var ps = Py.CreateScope())
                    {
                        Dictionary<string, object> inArguments = (from argument in Arguments
                                                                  where argument.Value.Direction != ArgumentDirection.Out
                                                                  select argument).ToDictionary((KeyValuePair<string, Argument> argument) => argument.Key, (KeyValuePair<string, Argument> argument) => argument.Value.Get(context));
                        foreach (var arg in inArguments)
                        {
                            ps.Set(arg.Key, arg.Value);
                        }

                        using (var scope = ps.NewScope())
                        {
                            PyObject pyObj = PythonPrintRedirectObject.Instance.ToPython();
                            dynamic sys = Py.Import("sys");
                            sys.stdout = pyObj;

                            dynamic os = Py.Import("os");
                            string workDir = PythonWorkingDirectory.Get(context);
                            if (string.IsNullOrEmpty(workDir))
                            {
                                os.chdir(SharedObject.Instance.ProjectPath);
                            }
                            else
                            {
                                os.chdir(workDir);
                            }

                            foreach (InArgument<string> item in SysPathAppendList)
                            {
                                sys.path.append(item.Get(context));
                            }


                            string pythonFilePath = PythonFilePath.Get(context);
                            scope.Exec(System.IO.File.ReadAllText(pythonFilePath));

                            //出参设置
                            Dictionary<string, object> outArguments = (from argument in Arguments
                                                                       where argument.Value.Direction != ArgumentDirection.In
                                                                       select argument).ToDictionary((KeyValuePair<string, Argument> argument) => argument.Key, (KeyValuePair<string, Argument> argument) => argument.Value.Get(context));

                            foreach (var arg in outArguments)
                            {
                                Type argumentType = Arguments[arg.Key].ArgumentType;

                                var argVal = scope.Get(arg.Key).AsManagedObject(argumentType);

                                if (arg.Value != null && !argumentType.IsAssignableFrom(argVal.GetType()))
                                {
                                    Arguments[arg.Key].Set(context, JsonDeserializeArgument(argVal, argumentType));
                                }
                                else
                                {
                                    Arguments[arg.Key].Set(context, argVal);
                                }
                            }
                        }
                    }
                }
            }
            catch (PythonException err)
            {
                string pythonFilePath = PythonFilePath.Get(context);
                var stackTrace = err.StackTrace.Replace("File \"<string>\"", $"File \"<{pythonFilePath}>\"");
                throw new Exception($"{err.Message}\r\n{stackTrace}");
            }
            catch (Exception err)
            {
                if (!flag)
                {
                    throw err;
                }
            }
            finally
            {
                if (ts != IntPtr.Zero)
                {
                    PythonEngine.EndAllowThreads(ts);
                }

                Environment.CurrentDirectory = prevCurrentDirectory;
            }
        }
    }
}