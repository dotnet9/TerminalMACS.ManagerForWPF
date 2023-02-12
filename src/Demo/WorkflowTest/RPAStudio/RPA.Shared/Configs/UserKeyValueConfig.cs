using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RPA.Shared.Configs
{
    public class UserKeyValueConfig
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private static bool _hasLoad = false;

        private static Dictionary<string, string> _keyValDict = new Dictionary<string, string>();

        public static event EventHandler<string> ValueChangedEvent;

        public static string GetValue(string keyName,string defaultVal = null)
        {
            if (!_hasLoad)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(AppPathConfig.StudioUserConfigXml);
                    var rootNode = doc.DocumentElement;
                    foreach (var item in rootNode.ChildNodes)
                    {
                        var element = item as XmlElement;
                        if(element?.NodeType == XmlNodeType.Element)
                        {
                            var key = element.GetAttribute("Key");
                            var val = element.GetAttribute("Value");
                            _keyValDict[key] = val;
                        }
                    }

                    _hasLoad = true;
                }
                catch (Exception err)
                {
                    _logger.Error(err);
                }
            }

            string ret = null;

            if (_keyValDict.ContainsKey(keyName))
            {
                ret = _keyValDict[keyName];
            }

            if(string.IsNullOrEmpty(ret))
            {
                return defaultVal;
            }

            return ret;
        }

        public static bool SetKeyValue(string keyName,string value)
        {
            //欲修改的值无须修改则直接返回
            if(GetValue(keyName) == value)
            {
                return true;
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppPathConfig.StudioUserConfigXml);
                var rootNode = doc.DocumentElement;

                bool hasFound = false;
                foreach (var item in rootNode.ChildNodes)
                {
                    var element = item as XmlElement;
                    if (element?.NodeType == XmlNodeType.Element)
                    {
                        var key = element.GetAttribute("Key");
                        if (key == keyName)
                        {
                            element.SetAttribute("Value", value);
                            hasFound = true;
                            break;
                        }
                    } 
                }

                if(!hasFound)
                {
                    XmlElement keyValueElement = doc.CreateElement("KeyValue");
                    keyValueElement.SetAttribute("Key", keyName);
                    keyValueElement.SetAttribute("Value", value);
                    rootNode.AppendChild(keyValueElement);
                }

                doc.Save(AppPathConfig.StudioUserConfigXml);

                if (_hasLoad)
                {
                    _keyValDict[keyName] = value;
                }

                ValueChangedEvent?.Invoke(null, keyName);
            }
            catch (Exception err)
            {
                _logger.Error(err);
                return false;
            }

            return true;
        }

    }
}
