using NLog;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace RPA.Shared.Configs
{
    public class SystemKeyValueConfig
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private static bool _hasLoad = false;

        private static Dictionary<string,string> _keyValDict = new Dictionary<string, string>();

        public static string GetValue(string keyName, string defaultVal = null)
        {
            if(!_hasLoad)
            {
                var xmlContent = Common.GetResourceContentByUri($"pack://application:,,,/RPA.Resources;Component/Configs/RPAStudio.System.xml");
                if(!string.IsNullOrEmpty(xmlContent))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    var rootNode = doc.DocumentElement;
                    foreach (var item in rootNode.ChildNodes)
                    {
                        var element = item as XmlElement;
                        if (element?.NodeType == XmlNodeType.Element)
                        {
                            var key = element.GetAttribute("Key");
                            var val = element.GetAttribute("Value");
                            _keyValDict[key] = val;
                        }
                    }

                    _hasLoad = true;
                }
            }

            string ret = null;

            if (_keyValDict.ContainsKey(keyName))
            {
                ret = _keyValDict[keyName];
            }

            if (string.IsNullOrEmpty(ret))
            {
                return defaultVal;
            }

            return ret;
        }
    }
}
