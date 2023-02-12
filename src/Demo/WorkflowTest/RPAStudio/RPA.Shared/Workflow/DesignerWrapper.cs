using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Workflow
{
    public static class DesignerWrapper
    {
        private static readonly List<KeyValuePair<Type, string>> s_specialCases = new List<KeyValuePair<Type, string>>
        {
            new KeyValuePair<Type, string>(typeof(TryCatch), "Try"),
            new KeyValuePair<Type, string>(typeof(TryCatch), "Finally"),
            new KeyValuePair<Type, string>(typeof(Catch<>), "Handler"),
            new KeyValuePair<Type, string>(typeof(DoWhile), "Body"),
            new KeyValuePair<Type, string>(typeof(ForEach<>), "Handler"),
            new KeyValuePair<Type, string>(typeof(If), "Then"),
            new KeyValuePair<Type, string>(typeof(If), "Else"),
            new KeyValuePair<Type, string>(typeof(Switch<>), "Default"),
            new KeyValuePair<Type, string>(typeof(ParallelForEach<>), "Body"),
            new KeyValuePair<Type, string>(typeof(ParallelForEach<>), "Handler"),
            new KeyValuePair<Type, string>(typeof(PickBranch), "Trigger"),
            new KeyValuePair<Type, string>(typeof(PickBranch), "Action"),
            new KeyValuePair<Type, string>(typeof(Transition), "Trigger"),
            new KeyValuePair<Type, string>(typeof(Transition), "Action"),
            new KeyValuePair<Type, string>(typeof(State), "Entry"),
            new KeyValuePair<Type, string>(typeof(State), "Exit"),
            new KeyValuePair<Type, string>(typeof(While), "Body")
        };

        public static bool IsWorkflowDesignerEmpty(WorkflowDesigner designer)
        {
            ModelService modelService = designer.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> activities = modelService.Find(modelService.Root, typeof(Activity));

            return activities.ToList().Count == 0;
        }

        public static void RemoveActivity(ModelItem activity)
        {
            Type left;
            if (activity == null)
            {
                left = null;
            }
            else
            {
                ModelItem parent = activity.Parent;
                left = ((parent != null) ? parent.ItemType : null);
            }
            if (left == typeof(ActivityBuilder))
            {
                ModelItem parent2 = activity.Parent;
                if (parent2 == null)
                {
                    return;
                }
                parent2.Content.ClearValue();
                return;
            }
            else
            {
                ModelItem parentActivity = activity.GetParentActivity();
                if (parentActivity == null)
                {
                    return;
                }
                using (ModelEditingScope modelEditingScope = parentActivity.BeginEdit())
                {
                    foreach (ModelProperty modelProperty in parentActivity.Properties)
                    {
                        if (modelProperty.IsCollection)
                        {
                            if (activity.Parent.GetCurrentValue() is FlowStep)
                            {
                                if (modelProperty.Collection.Contains(activity.Parent.GetCurrentValue()))
                                {
                                    modelProperty.Collection.Remove(activity.Parent.GetCurrentValue());
                                    break;
                                }
                            }

                            if (modelProperty.Collection.Contains(activity))
                            {
                                modelProperty.Collection.Remove(activity);
                                break;
                            }
                        }
                        else if (modelProperty.Value == activity)
                        {
                            modelProperty.ClearValue();
                            break;
                        }
                    }
                    modelEditingScope.Complete();
                }
                return;
            }
        }


        public static void SetDisplayName(ModelItem modelItem, string currentDraggingDisplayName)
        {
            Type activity = modelItem.ItemType;
            object currentValue = modelItem.GetCurrentValue();
            if (modelItem.IsState() && (currentValue as State).IsFinal)
            {
                activity = typeof(FinalState);
            }
            string newValue = currentDraggingDisplayName;
            new PropertyValueChange
            {
                PropertyName = "DisplayName",
                NewValue = newValue,
                ActivityToChange = modelItem
            }.Apply();
        }

        public static bool IsSpecialProperty(ModelChangeInfo changeInfo)
        {
            bool flag3;
            try
            {
                if (changeInfo.PropertyName == "Condition")
                {
                    ModelItem value = changeInfo.Value;
                    bool flag;
                    if (value == null)
                    {
                        flag = false;
                    }
                    else
                    {
                        Type type = value.GetType();
                        bool? flag2 = (type != null) ? new bool?(type.IsSubclassOf(typeof(Activity))) : null;
                        flag3 = true;
                        flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
                    }
                    if (flag)
                    {
                        return true;
                    }
                }
                if (changeInfo.PropertyName == "AnchorProvider")
                {
                    ModelItem value2 = changeInfo.Value;
                    if (value2 != null && value2.IsActivity())
                    {
                        return true;
                    }
                }
                Type type2 = changeInfo.Value.GetType();
                bool flag4;
                if (type2 == null)
                {
                    flag4 = false;
                }
                else
                {
                    Type baseType = type2.BaseType;
                    bool? flag2 = (baseType != null) ? new bool?(baseType.Equals(typeof(ActivityWithResult))) : null;
                    bool flag5 = false;
                    flag4 = (flag2.GetValueOrDefault() == flag5 & flag2 != null);
                }
                if (flag4)
                {
                    if (changeInfo.PropertyName == "Value")
                    {
                        try
                        {
                            ModelItem parent = changeInfo.Value.Parent.Parent.Parent.Parent;
                            return parent.ItemType.IsGenericType && parent.ItemType.GetGenericTypeDefinition().Equals(typeof(Switch<>));
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    foreach (KeyValuePair<Type, string> keyValuePair in DesignerWrapper.s_specialCases)
                    {
                        if (changeInfo.PropertyName == keyValuePair.Value)
                        {
                            ModelItem parent2 = changeInfo.Value.Parent;
                            if (parent2.ItemType.Equals(keyValuePair.Key) || (parent2.ItemType.IsGenericType && parent2.ItemType.GetGenericTypeDefinition().Equals(keyValuePair.Key)))
                            {
                                return true;
                            }
                            parent2 = parent2.Parent;
                            if (parent2 != null && (parent2.ItemType.Equals(keyValuePair.Key) || (parent2.ItemType.IsGenericType && parent2.ItemType.GetGenericTypeDefinition().Equals(keyValuePair.Key))))
                            {
                                return true;
                            }
                        }
                    }
                    if (changeInfo.PropertyName == "Handler")
                    {
                        return true;
                    }
                }
                flag3 = false;
            }
            catch
            {
                flag3 = false;
            }
            return flag3;
        }


    }
}
