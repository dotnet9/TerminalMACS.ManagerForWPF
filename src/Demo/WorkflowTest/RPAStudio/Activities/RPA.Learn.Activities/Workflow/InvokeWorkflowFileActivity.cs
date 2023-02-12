using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using RPA.Interfaces.Share;
using System.Activities.Presentation.Metadata;
using Activities.Shared.Editors;
using System.Activities.Presentation.PropertyEditing;
using RPA.Shared.Utils;
using System.Activities.XamlIntegration;
using RPA.Services.Workflow;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RPA.Learn.Activities.Workflow
{
    [Designer(typeof(InvokeWorkflowFileDesigner))]
    public sealed class InvokeWorkflowFileActivity : CodeActivity
    {
        private string ProjectPath { get; set; }

        [Category("输入")]
        [RequiredArgument]
        [Description("工作流文件路径，必须用双引号括起来")]
        [DisplayName("工作流文件路径")]
        public InArgument<string> WorkflowFilePath { get; set; }

        [Category("输入")]
        [Browsable(true)]
        [DisplayName("参数")]
        public Dictionary<string, Argument> Arguments { get; private set; } = new Dictionary<string, Argument>();

        public InvokeWorkflowFileActivity()
        {
            ProjectPath = SharedObject.Instance.ProjectPath;

            var builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(typeof(InvokeWorkflowFileActivity), "Arguments"
                , new EditorAttribute(typeof(DictionaryArgumentEditor), typeof(DialogPropertyValueEditor)));
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }


        public void SetFilePath(string filePath)
        {
            if (filePath.StartsWith(SharedObject.Instance.ProjectPath, System.StringComparison.CurrentCultureIgnoreCase))
            {
                //如果在项目目录下，则使用相对路径保存
                filePath = Common.MakeRelativePath(SharedObject.Instance.ProjectPath, filePath);
            }

            WorkflowFilePath = filePath;
        }


        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            foreach (KeyValuePair<string, Argument> argument2 in Arguments)
            {
                Argument value = argument2.Value;
                RuntimeArgument argument = new RuntimeArgument(argument2.Key, value.ArgumentType, value.Direction);
                metadata.Bind(value, argument);
                metadata.AddArgument(argument);
            }

            base.CacheMetadata(metadata);
        }

        public static object JsonDeserializeArgument(object value, Type argumentType)
        {
            return JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(value)).ToObject(argumentType);
        }

        protected override void Execute(CodeActivityContext context)
        {
            string workflowFilePath = context.GetValue(this.WorkflowFilePath);

            if (!System.IO.Path.IsPathRooted(workflowFilePath))
            {
                workflowFilePath = System.IO.Path.Combine(ProjectPath, workflowFilePath);
            }

            try
            {
                Dictionary<string, object> inArguments = (from argument in Arguments
                                                          where argument.Value.Direction != ArgumentDirection.Out
                                                          select argument).ToDictionary((KeyValuePair<string, Argument> argument) => argument.Key, (KeyValuePair<string, Argument> argument) => argument.Value.Get(context));

                Activity workflow = ActivityXamlServices.Load(workflowFilePath);

                var invoker = new WorkflowInvoker(workflow);

                invoker.Extensions.Add(new LogToOutputWindowTextWriter());

                if (workflow is DynamicActivity)
                {
                    var wr = new WorkflowRuntime();
                    wr.RootActivity = workflow;
                    invoker.Extensions.Add(wr);
                }

                var outputArguments = invoker.Invoke(inArguments);

                foreach (KeyValuePair<string, object> item in from argument in outputArguments
                                                              where Arguments.ContainsKey(argument.Key)
                                                              select argument)
                {
                    Type argumentType = Arguments[item.Key].ArgumentType;
                    if (item.Value != null && !argumentType.IsAssignableFrom(item.Value.GetType()))
                    {
                        Arguments[item.Key].Set(context, JsonDeserializeArgument(item.Value, argumentType));
                    }
                    else
                    {
                        Arguments[item.Key].Set(context, item.Value);
                    }
                }

            }
            catch (System.Exception e)
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, e.ToString());
                throw;
            }

        }
    }
}