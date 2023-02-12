using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Workflow
{
    /// <summary>
    /// ModelItem函数扩展
    /// </summary>
    public static class ModelItemExtensions
    {
        public static bool IsType<T>(this ModelItem modelItem)
        {
            return typeof(T).IsAssignableFrom(modelItem.ItemType);
        }

        public static bool IsFlowNode(this ModelItem modelItem)
        {
            return modelItem.ItemType.BaseType == typeof(FlowNode);
        }

        public static bool IsState(this ModelItem modelItem)
        {
            return modelItem.IsType<State>();
        }

        public static bool IsStateMachine(this ModelItem modelItem)
        {
            return modelItem.ItemType == typeof(StateMachine);
        }

        public static bool IsActivity(this ModelItem modelItem)
        {
            return modelItem.IsType<Activity>();
        }

        public static bool IsFlowchart(this ModelItem modelItem)
        {
            return modelItem.ItemType == typeof(Flowchart);
        }

        public static bool IsFlowDecision(this ModelItem modelItem)
        {
            return modelItem.ItemType == typeof(FlowDecision);
        }

        public static bool IsFlowStep(this ModelItem modelItem)
        {
            return modelItem.ItemType == typeof(FlowStep);
        }

        public static bool IsFlowSwitch(this ModelItem modelItem)
        {
            if (modelItem.ItemType.IsGenericType)
            {
                return modelItem.ItemType.GetGenericTypeDefinition() == typeof(FlowSwitch<>);
            }
            return false;
        }

        public static bool IsPickBranch(this ModelItem modelItem)
        {
            return modelItem.ItemType == typeof(PickBranch);
        }

        public static bool IsSequence(this ModelItem modelItem)
        {
            return modelItem.ItemType == typeof(Sequence);
        }

        public static bool IsTransition(this ModelItem modelItem)
        {
            return modelItem.IsType<Transition>();
        }

        public static bool IsVariable(this ModelItem modelItem)
        {
            return modelItem.IsType<Variable>();
        }

        //是否能添加条目
        public static bool CanAddItem(this ModelItem modelItem, object item)
        {
            if (item == null)
            {
                return false;
            }
            if (item is Activity)
            {
                if (!modelItem.IsSequence())
                {
                    return modelItem.IsFlowchart();
                }
                return true;
            }
            if (item is State || item is FinalState)
            {
                return modelItem.IsStateMachine();
            }
            if (item is FlowNode)
            {
                return modelItem.IsFlowchart();
            }
            return false;
        }

        //添加活动
        public static ModelItem AddActivity(this ModelItem modelItem, object item, int index = -1)
        {
            if (!modelItem.CanAddItem(item))
            {
                return null;
            }
            ModelItem result = null;
            Activity value;
            if (modelItem.IsSequence() && (value = (item as Activity)) != null)
            {
                ModelItemCollection collection = modelItem.Properties["Activities"].Collection;
                result = ((index == -1) ? collection.Add(value) : collection.Insert(index, value));
            }
            else if (modelItem.IsFlowchart())
            {
                FlowNode flowNode = null;
                if (item != null)
                {
                    FlowNode flowNode2;
                    if ((flowNode2 = (item as FlowNode)) == null)
                    {
                        Activity activity;
                        if ((activity = (item as Activity)) != null)
                        {
                            Activity action = activity;
                            flowNode = new FlowStep
                            {
                                Action = action
                            };
                        }
                    }
                    else
                    {
                        FlowNode flowNode3 = flowNode2;
                        flowNode = flowNode3;
                    }
                }
                if (flowNode != null)
                {
                    ModelItemCollection collection2 = modelItem.Properties["Nodes"].Collection;
                    result = ((index == -1) ? collection2.Add(flowNode) : collection2.Insert(index, flowNode));
                }
            }
            else if (modelItem.IsStateMachine() && (item is State || item is FinalState))
            {
                State state = (item as State) ?? new State
                {
                    IsFinal = true,
                };

                ModelItemCollection collection3 = modelItem.Properties["States"].Collection;
                result = ((index == -1) ? collection3.Add(state) : collection3.Insert(index, state));
            }
            return result;
        }


        public static ModelItem GetParentActivity(this ModelItem modelItem)
        {
            ModelItem modelItem2 = (modelItem != null) ? modelItem.Parent : null;
            while (modelItem2 != null && !typeof(Activity).IsAssignableFrom(modelItem2.ItemType))
            {
                modelItem2 = modelItem2.Parent;
            }
            return modelItem2;
        }

    }
}
