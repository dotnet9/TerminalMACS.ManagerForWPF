using RPA.Interfaces.Activities;
using RPA.Shared.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RPA.Services.Activities
{
    public class ActivityRecentService : IActivityRecentService
    {
        public ActivityRecentService()
        {
        }

        public void Add(string typeOf)
        {
            XmlDocument doc = new XmlDocument();
            var path = AppPathConfig.RecentActivitiesXml;
            doc.Load(path);
            var rootNode = doc.DocumentElement;

            var activityNodes = rootNode.SelectNodes("Activity");
            //最多记录N条
            while (activityNodes.Count >= ProjectConstantConfig.ActivitiesRecentGroupMaxRecordCount)
            {
                rootNode.RemoveChild(rootNode.LastChild);
                activityNodes = rootNode.SelectNodes("Activity");
            }

            foreach (XmlElement item in activityNodes)
            {
                var typeOfStr = item.GetAttribute("TypeOf");
                if (typeOfStr == typeOf)
                {
                    rootNode.RemoveChild(item);
                    break;
                }
            }

            XmlElement activityElement = doc.CreateElement("Activity");
            activityElement.SetAttribute("TypeOf", typeOf);
            rootNode.PrependChild(activityElement);

            doc.Save(path);
        }

        public List<ActivityGroupOrLeafItem> Query()
        {
            var ret = new List<ActivityGroupOrLeafItem>();

            XmlDocument doc = new XmlDocument();
            doc.Load(AppPathConfig.RecentActivitiesXml);
            var rootNode = doc.DocumentElement;

            var activitiesNodes = rootNode.SelectNodes("Activity");
            foreach (XmlNode activityNode in activitiesNodes)
            {
                var TypeOf = (activityNode as XmlElement).GetAttribute("TypeOf");
                ActivityLeafItem item = new ActivityLeafItem();
                item.TypeOf = TypeOf;
                ret.Add(item);
            }

            return ret;
        }
    }
}
