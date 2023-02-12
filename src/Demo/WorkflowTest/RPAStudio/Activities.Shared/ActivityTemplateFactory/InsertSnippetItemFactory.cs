using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Debugger.Symbol;
using System.Activities.Presentation;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xaml;

namespace Activities.Shared.ActivityTemplateFactory
{
    public class InsertSnippetItemFactory : IActivityTemplateFactory
    {
        public static readonly string AssemblyQualifiedName = typeof(InsertSnippetItemFactory).AssemblyQualifiedName;

        public static string FilePath { get; set; }

        public Activity Create(DependencyObject target)
        {
            try
            {
                Activity activity = ActivityXamlServices.Load(FilePath);
                AttachableMemberIdentifier name = new AttachableMemberIdentifier(typeof(DebugSymbol), "Symbol");
                try
                {
                    AttachablePropertyServices.RemoveProperty(activity, name);
                }
                catch
                {
                }
                DynamicActivity dynamicActivity = activity as DynamicActivity;
                if (dynamicActivity != null)
                {
                    ActivityBuilder activityBuilder = new ActivityBuilder();
                    activityBuilder.Implementation = ((dynamicActivity.Implementation != null) ? dynamicActivity.Implementation() : null);
                    activityBuilder.Name = dynamicActivity.Name;
                    if (activityBuilder.Implementation != null)
                    {
                        try
                        {
                            AttachablePropertyServices.RemoveProperty(activityBuilder.Implementation, name);
                        }
                        catch
                        {
                        }
                    }
                    foreach (Attribute item in dynamicActivity.Attributes)
                    {
                        activityBuilder.Attributes.Add(item);
                    }
                    foreach (Constraint item2 in dynamicActivity.Constraints)
                    {
                        activityBuilder.Constraints.Add(item2);
                    }
                    foreach (DynamicActivityProperty dynamicActivityProperty in dynamicActivity.Properties)
                    {
                        DynamicActivityProperty dynamicActivityProperty2 = new DynamicActivityProperty
                        {
                            Name = dynamicActivityProperty.Name,
                            Type = dynamicActivityProperty.Type,
                            Value = null
                        };
                        foreach (Attribute item3 in dynamicActivityProperty.Attributes)
                        {
                            dynamicActivityProperty2.Attributes.Add(item3);
                        }
                        activityBuilder.Properties.Add(dynamicActivityProperty2);
                    }
                    VisualBasic.SetSettings(activityBuilder, VisualBasic.GetSettings(dynamicActivity));
                    return activityBuilder.Implementation;
                }
                return activity;
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
    }
}
