using RPA.Interfaces.Activities;
using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RPA.Services.Activities
{
    public class ActivitiesDefaultAttributesService : IActivitiesDefaultAttributesService
    {
        private readonly string ActivityBuilderImplementationVersion = "实现版本";
        private readonly string BasicCategoryName = "基本";
        private readonly string ActivityBuilderName = "显示名称";
        private readonly string Result = "结果";
        private readonly string OutputCategoryName = "输出";
        private readonly string DisplayNamePropertyName = "显示名称";
        private readonly string ConditionPropertyName = "条件";
        private readonly string InputCategoryName = "输入";
        private readonly string CollectionDisplayName = "集合";
        private readonly string ItemDisplayName = "项";
        private readonly string CollectionResultDisplayName = "结果";

        private readonly string WriteLineTextDisplayName = "文本";
        private readonly string WriteLineTextWriterDisplayName = "文本重定向";
        private readonly string GenericTypeArgumentsDisplayName = "通用类型参数";
        private readonly string MethodNameDisplayName = "方法名称";
        private readonly string ParametersDisplayName = "参数";
        private readonly string InvokeMethodResultDisplayName = "结果";
        private readonly string RunAsynchronouslyDisplayName = "异步运行";
        private readonly string TargetObjectDisplayName = "目标对象";
        private readonly string TargetTypeDisplayName = "目标类型";
        private readonly string FlowDecisionConditionDisplayName = "条件";
        private readonly string FlowDecisionTrueDisplayName = "True";
        private readonly string FlowDecisionFalseDisplayName = "False";
        private readonly string FlowDecisionExpressionDisplayName = "表达式";
        private readonly string FlowchartValidateUnconnectedNodesDisplayName = "验证未连接的节点";
        private readonly string TerminateWorkflowExceptionDisplayName = "异常";
        private readonly string TerminateWorkflowReasonDisplayName = "原因";
        private readonly string ThrowExceptionDisplayName = "异常";
        private readonly string AssignToDisplayName = "目标";
        private readonly string AssignValueDisplayName = "值";
        private readonly string DelayDurationDisplayName = "持续时间";
        private readonly string WhileConditionDisplayName = "条件";
        private readonly string DoWhileConditionDisplayName = "条件";
        private readonly string IfConditionDisplayName = "条件";
        private readonly string ParallelCompletionConditionDisplayName = "条件";
        private readonly string ParallelForEachCompletionConditionDisplayName = "条件";

        private readonly string ParallelForEachValuesDisplayName = "值";
        private readonly string SwitchExpressionDisplayName = "表达式";

        public ActivitiesDefaultAttributesService()
        {
            new DesignerMetadata().Register();
        }

        public void Register()
        {
            AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();

            AddDefaultNameAttribute(attributeTableBuilder);
            AddGenericTypeEditor(attributeTableBuilder);

            MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());
        }

        private void AddDefaultNameAttribute(AttributeTableBuilder builder)
        {
            builder.AddCustomAttributes(typeof(ActivityBuilder), "ImplementationVersion", new Attribute[]
            {
                new DisplayNameAttribute(ActivityBuilderImplementationVersion),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(ActivityBuilder), "Name", new Attribute[]
            {
                new DisplayNameAttribute(ActivityBuilderName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(Activity<>), "Result", new Attribute[]
            {
                new DisplayNameAttribute(Result),
                new CategoryAttribute(OutputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Activity), "DisplayName", new Attribute[]
            {
                new DisplayNameAttribute(DisplayNamePropertyName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(FlowDecision), "DisplayName", new Attribute[]
            {
                new DisplayNameAttribute(DisplayNamePropertyName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(FlowSwitch<>), "DisplayName", new Attribute[]
            {
                new DisplayNameAttribute(DisplayNamePropertyName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(PickBranch), "DisplayName", new Attribute[]
            {
                new DisplayNameAttribute(DisplayNamePropertyName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(Transition), "DisplayName", new Attribute[]
            {
                new DisplayNameAttribute(DisplayNamePropertyName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(Transition), "Condition", new Attribute[]
            {
                new DisplayNameAttribute(ConditionPropertyName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(State), "DisplayName", new Attribute[]
            {
                new DisplayNameAttribute(DisplayNamePropertyName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(AddToCollection<>), "Collection", new Attribute[]
            {
                new DisplayNameAttribute(CollectionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(AddToCollection<>), "Item", new Attribute[]
            {
                new DisplayNameAttribute(ItemDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(ClearCollection<>), "Collection", new Attribute[]
            {
                new DisplayNameAttribute(CollectionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(ExistsInCollection<>), "Collection", new Attribute[]
            {
                new DisplayNameAttribute(CollectionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(ExistsInCollection<>), "Item", new Attribute[]
            {
                new DisplayNameAttribute(ItemDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(ExistsInCollection<>), "Result", new Attribute[]
            {
                new DisplayNameAttribute(CollectionResultDisplayName),
                new CategoryAttribute(OutputCategoryName)
            });
            builder.AddCustomAttributes(typeof(RemoveFromCollection<>), "Collection", new Attribute[]
            {
                new DisplayNameAttribute(CollectionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(RemoveFromCollection<>), "Item", new Attribute[]
            {
                new DisplayNameAttribute(ItemDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(RemoveFromCollection<>), "Result", new Attribute[]
            {
                new DisplayNameAttribute(CollectionResultDisplayName),
                new CategoryAttribute(OutputCategoryName)
            });
            builder.AddCustomAttributes(typeof(WriteLine), "Text", new Attribute[]
            {
                new DisplayNameAttribute(WriteLineTextDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(WriteLine), "TextWriter", new Attribute[]
            {
                new DisplayNameAttribute(WriteLineTextWriterDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(InvokeMethod), "GenericTypeArguments", new Attribute[]
            {
                new DisplayNameAttribute(GenericTypeArgumentsDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(InvokeMethod), "MethodName", new Attribute[]
            {
                new DisplayNameAttribute(MethodNameDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(InvokeMethod), "Parameters", new Attribute[]
            {
                new DisplayNameAttribute(ParametersDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(InvokeMethod), "Result", new Attribute[]
            {
                new DisplayNameAttribute(InvokeMethodResultDisplayName),
                new CategoryAttribute(OutputCategoryName)
            });
            builder.AddCustomAttributes(typeof(InvokeMethod), "RunAsynchronously", new Attribute[]
            {
                new DisplayNameAttribute(RunAsynchronouslyDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(InvokeMethod), "TargetObject", new Attribute[]
            {
                new DisplayNameAttribute(TargetObjectDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(InvokeMethod), "TargetType", new Attribute[]
            {
                new DisplayNameAttribute(TargetTypeDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(FlowDecision), "Condition", new Attribute[]
            {
                new DisplayNameAttribute(FlowDecisionConditionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(FlowDecision), "True", new Attribute[]
            {
                new DisplayNameAttribute(FlowDecisionTrueDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(FlowDecision), "False", new Attribute[]
            {
                new DisplayNameAttribute(FlowDecisionFalseDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(FlowSwitch<>), "Expression", new Attribute[]
            {
                new DisplayNameAttribute(FlowDecisionExpressionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Flowchart), "ValidateUnconnectedNodes", new Attribute[]
            {
                new DisplayNameAttribute(FlowchartValidateUnconnectedNodesDisplayName),
                new CategoryAttribute(BasicCategoryName)
            });
            builder.AddCustomAttributes(typeof(TerminateWorkflow), "Exception", new Attribute[]
            {
                new DisplayNameAttribute(TerminateWorkflowExceptionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(TerminateWorkflow), "Reason", new Attribute[]
            {
                new DisplayNameAttribute(TerminateWorkflowReasonDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Throw), "Exception", new Attribute[]
            {
                new DisplayNameAttribute(ThrowExceptionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Assign), "To", new Attribute[]
            {
                new DisplayNameAttribute(AssignToDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Assign), "Value", new Attribute[]
            {
                new DisplayNameAttribute(AssignValueDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Delay), "Duration", new Attribute[]
            {
                new DisplayNameAttribute(DelayDurationDisplayName),
                new CategoryAttribute(InputCategoryName)
            });

            builder.AddCustomAttributes(typeof(While), "Condition", new Attribute[]
            {
                new DisplayNameAttribute(WhileConditionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(DoWhile), "Condition", new Attribute[]
            {
                new DisplayNameAttribute(DoWhileConditionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(If), "Condition", new Attribute[]
            {
                new DisplayNameAttribute(IfConditionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Parallel), "CompletionCondition", new Attribute[]
            {
                new DisplayNameAttribute(ParallelCompletionConditionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(ParallelForEach<>), "CompletionCondition", new Attribute[]
            {
                new DisplayNameAttribute(ParallelForEachCompletionConditionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(ParallelForEach<>), "Values", new Attribute[]
            {
                new DisplayNameAttribute(ParallelForEachValuesDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
            builder.AddCustomAttributes(typeof(Switch<>), "Expression", new Attribute[]
            {
                new DisplayNameAttribute(SwitchExpressionDisplayName),
                new CategoryAttribute(InputCategoryName)
            });
        }

        private void AddGenericTypeEditor(AttributeTableBuilder builder)
        {
            Type type = Type.GetType("System.Activities.Presentation.FeatureAttribute, System.Activities.Presentation, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            Type type2 = Type.GetType("System.Activities.Presentation.UpdatableGenericArgumentsFeature, System.Activities.Presentation, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            Attribute attribute = Activator.CreateInstance(type, new object[]
            {
                type2
            }) as Attribute;
            builder.AddCustomAttributes(typeof(FlowSwitch<>), new Attribute[]
            {
                attribute
            });

            builder.AddCustomAttributes(typeof(Switch<>), new Attribute[]
            {
                attribute
            });
            builder.AddCustomAttributes(typeof(AddToCollection<>), new Attribute[]
            {
                new DefaultTypeArgumentAttribute(typeof(object))
            });
            builder.AddCustomAttributes(typeof(ClearCollection<>), new Attribute[]
            {
                new DefaultTypeArgumentAttribute(typeof(object))
            });
            builder.AddCustomAttributes(typeof(ExistsInCollection<>), new Attribute[]
            {
                new DefaultTypeArgumentAttribute(typeof(object))
            });
            builder.AddCustomAttributes(typeof(RemoveFromCollection<>), new Attribute[]
            {
                new DefaultTypeArgumentAttribute(typeof(object))
            });

            builder.AddCustomAttributes(typeof(Switch<>), new Attribute[]
            {
                new DefaultTypeArgumentAttribute(typeof(string))
            });
            builder.AddCustomAttributes(typeof(FlowSwitch<>), new Attribute[]
            {
                new DefaultTypeArgumentAttribute(typeof(int))
            });
        }

    }
}
