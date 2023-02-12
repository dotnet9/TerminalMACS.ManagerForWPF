using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt.Implementation;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RPA.Services.ExpressionEditor
{
    public class VariablesSession
    {
        public CompletionSession CompletionSession { get; }

        public Type ExpressionType { get; }

        public ModelItem PrimarySelection { get; }

        public Dictionary<string, Type> Variables { get; }

        public VariablesSession(List<ModelItem> variables, Type expressionType, EditingContext editingContext)
        {
            this.ExpressionType = expressionType;
            EditingContext editingContext2 = (variables.Count > 0) ? variables[0].GetEditingContext() : editingContext;
            if (editingContext2 != null)
            {
                this.PrimarySelection = editingContext2.Items.GetValue<Selection>().PrimarySelection;
            }
            this.Variables = VariablesSession.GetVariables(variables);
            if (this.PrimarySelection != null)
            {
                IEnumerable<Activity> workflowActivities = VariablesSession.GetWorkflowActivities(editingContext2);
                Activity activity = this.PrimarySelection.GetCurrentValue() as Activity;
                try
                {
                    Dictionary<string, Activity> variableActivityMap = this.GetVariableActivityMap(workflowActivities);
                    if (variableActivityMap.Count != 0)
                    {
                        this.CompletionSession = new CompletionSession
                        {
                            MatchOptions = (CompletionMatchOptions.IsCaseInsensitive | CompletionMatchOptions.TargetsDisplayText)
                        };
                        foreach (KeyValuePair<string, Activity> keyValuePair in variableActivityMap)
                        {
                            if (keyValuePair.Value == null || activity != keyValuePair.Value)
                            {
                                this.CompletionSession.Items.Add(this.CreateWorkflowVariableData(keyValuePair.Key, keyValuePair.Value, this.Variables[keyValuePair.Key]));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        private static Dictionary<string, Type> GetVariables(IEnumerable<ModelItem> items)
        {
            Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
            foreach (ModelItem modelItem in items)
            {
                ModelProperty modelProperty = modelItem.Properties["Name"];
                string text = ((modelProperty != null) ? modelProperty.ComputedValue : null) as string;
                if (!string.IsNullOrEmpty(text) && !(text == "ContextTarget") && !(text == "ContextAnchor"))
                {
                    ModelProperty modelProperty2 = modelItem.Properties["Type"];
                    Type type = ((modelProperty2 != null) ? modelProperty2.ComputedValue : null) as Type;
                    if (!(type == null))
                    {
                        dictionary[text] = type;
                    }
                }
            }
            return dictionary;
        }

        private static IEnumerable<Activity> GetWorkflowActivities(EditingContext editingContext)
        {
            ModelService modelService = (editingContext != null) ? editingContext.Services.GetService<ModelService>() : null;
            if (modelService == null)
            {
                return null;
            }
            return from x in modelService.Find(modelService.Root, typeof(Activity))
                   select x.GetCurrentValue() as Activity;
        }


        private Dictionary<string, Activity> GetVariableActivityMap(IEnumerable<Activity> activities)
        {
            Dictionary<string, Activity> matchingVariables = this.GetMatchingVariables();
            if (matchingVariables == null || matchingVariables.Count == 0)
            {
                return matchingVariables;
            }
            foreach (Activity activity in activities)
            {
                foreach (PropertyInfo propertyInfo in from x in activity.GetType().GetProperties()
                                                      where x.PropertyType.BaseType == typeof(OutArgument)
                                                      select x)
                {
                    try
                    {
                        OutArgument outArgument = propertyInfo.GetValue(activity, null) as OutArgument;
                        if (((outArgument != null) ? outArgument.Expression : null) != null)
                        {
                            ActivityWithResult expression = outArgument.Expression;
                            string text = null;
                            if (expression.GetType().GetProperty("ExpressionText") != null)
                            {
                                PropertyInfo property = expression.GetType().GetProperty("ExpressionText");
                                text = (((property != null) ? property.GetValue(expression) : null) as string);
                            }
                            else if (expression.GetType().GetProperty("Variable") != null)
                            {
                                PropertyInfo property2 = expression.GetType().GetProperty("Variable");
                                Variable variable = ((property2 != null) ? property2.GetValue(expression) : null) as Variable;
                                text = ((variable != null) ? variable.Name : null);
                            }
                            if (!string.IsNullOrEmpty(text) && matchingVariables.ContainsKey(text))
                            {
                                matchingVariables[text] = activity;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.ToString());
                    }
                }
            }
            return matchingVariables;
        }

        private Dictionary<string, Activity> GetMatchingVariables()
        {
            return (from x in this.Variables
                    where this.ExpressionType == typeof(void) || this.ExpressionType.IsAssignableFrom(x.Value) || (x.Value == typeof(object) && this.ExpressionType == typeof(string)) || (!(x.Value.Name != "GenericValue") && (this.ExpressionType == typeof(TimeSpan) || Type.GetTypeCode(this.ExpressionType) != TypeCode.Object))
                    select x).ToDictionary((Func<KeyValuePair<string, Type>, string>)((KeyValuePair<string, Type> x) => x.Key), (Func<KeyValuePair<string, Type>, Activity>)((KeyValuePair<string, Type> x) => null));
        }



        private CompletionItem CreateWorkflowVariableData(string variableName, Activity variableSource, Type variableType)
        {
            string text = variableName;
            string text2 = string.Empty;
            if (this.ExpressionType == typeof(string) && variableType == typeof(object))
            {
                text += ".ToString";
            }
            if (variableSource != null)
            {
                text2 = string.Format(@"输出自{0}", variableSource.DisplayName);
            }
            text2 = text2 + variableType.FriendlyName(false, false) + " " + variableName;
            return new CompletionItem(text, new CommonImageSourceProvider(CommonImageKind.FieldPublic), new DirectContentProvider(text2));
        }
    }
}