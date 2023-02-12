using RPA.Interfaces.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RPA.Services.Activities
{
    public class ActivityMountService : IActivityMountService
    {
        public List<ActivityGroupOrLeafItem> Transform(string activityConfigXml)
        {
            var ret = new List<ActivityGroupOrLeafItem>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(activityConfigXml);
            var rootNode = doc.DocumentElement;

            InitChildren(rootNode, ref ret);

            return ret;
        }

        private void InitChildren(XmlNode node, ref List<ActivityGroupOrLeafItem> children)
        {
            var groupNodes = node.SelectNodes("Group");
            foreach (XmlNode groupNode in groupNodes)
            {
                var name = (groupNode as XmlElement).GetAttribute("Name");
                ActivityGroupItem item = new ActivityGroupItem();
                item.Name = name;

                InitChildren(groupNode, ref item.Children);

                children.Add(item);
            }

            var activitiesNodes = node.SelectNodes("Activity");
            foreach (XmlNode activityNode in activitiesNodes)
            {
                ActivityLeafItem item = new ActivityLeafItem();

                item.Name = (activityNode as XmlElement).GetAttribute("Name");
                item.TypeOf = (activityNode as XmlElement).GetAttribute("TypeOf");
                item.ToolTip = (activityNode as XmlElement).GetAttribute("ToolTip");
                item.Icon = (activityNode as XmlElement).GetAttribute("Icon");

                children.Add(item);
            }
        }


        public List<ActivityGroupOrLeafItem> Mount(List<ActivityGroupOrLeafItem> activitiesCurrent
            , List<ActivityGroupOrLeafItem> activitiesToMount)
        {
            List<ActivityGroupOrLeafItem> ret = new List<ActivityGroupOrLeafItem>();

            Mount(activitiesCurrent, activitiesToMount, ref ret);

            return ret;
        }


        private void Mount(List<ActivityGroupOrLeafItem> activitiesCurrent
            , List<ActivityGroupOrLeafItem> activitiesToMount, ref List<ActivityGroupOrLeafItem> ret)
        {
            //结果集中先添加itemCurrent的所有组和叶子项
            foreach (var itemCurrent in activitiesCurrent)
            {
                ret.Add(itemCurrent);
            }

            //挂载项如果是叶子节点，直接挂进去
            foreach (var itemMount in activitiesToMount)
            {
                if (itemMount is ActivityLeafItem)
                {
                    ret.Add(itemMount);
                }
            }

            //分组节点合并后添加进去
            foreach (var itemMount in activitiesToMount)
            {
                if (itemMount is ActivityGroupItem)
                {
                    bool isGroupNameExist = false;

                    ActivityGroupItem itemMountAt = itemMount as ActivityGroupItem;
                    ActivityGroupItem itemCurrentAt = null;

                    foreach (var itemCurrent in activitiesCurrent)
                    {
                        if (itemCurrent is ActivityGroupItem && itemCurrent.Name == itemMount.Name)
                        {
                            isGroupNameExist = true;
                            itemCurrentAt = itemCurrent as ActivityGroupItem;
                            break;
                        }
                    }

                    if (isGroupNameExist)
                    {
                        //组名称存在，替换掉结果集中同名的组并继续往下走
                        foreach (var item in ret)
                        {
                            if (item is ActivityGroupItem && item.Name == itemMount.Name)
                            {
                                ret.Remove(item);
                                break;
                            }
                        }

                        ActivityGroupItem itemTemp = new ActivityGroupItem();
                        itemTemp.Name = itemCurrentAt.Name;
                        ret.Add(itemTemp);

                        Mount(itemCurrentAt.Children, itemMountAt.Children, ref itemTemp.Children);
                    }
                    else
                    {
                        //组名称不存在，直接挂在当前组下
                        ret.Add(itemMount);
                    }
                }
            }
        }
    }
}
