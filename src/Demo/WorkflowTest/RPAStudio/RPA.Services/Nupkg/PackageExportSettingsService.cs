using RPA.Interfaces.Nupkg;
using RPA.Shared.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RPA.Services.Nupkg
{
    public class PackageExportSettingsService : IPackageExportSettingsService
    {
        private const string _exportDirKey = "PackageExport.LastExportDirectory";

        public string GetLastExportDir()
        {
            return UserKeyValueConfig.GetValue(_exportDirKey);
        }

        public bool SetLastExportDir(string dir)
        {
            return UserKeyValueConfig.SetKeyValue(_exportDirKey, dir);
        }

        public bool AddToExportDirHistoryList(string dir)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                var path = AppPathConfig.RecentExportPathsXml;
                doc.Load(path);
                var rootNode = doc.DocumentElement;

                var items = rootNode.SelectNodes("Item");

                if (items.Count > ProjectConstantConfig.ExportDirHistoryMaxRecordCount)
                {
                    rootNode.RemoveChild(rootNode.LastChild);
                }

                foreach (XmlElement item in items)
                {
                    if (dir.ToLower() == item.InnerText.ToLower())
                    {
                        rootNode.RemoveChild(item);
                        break;
                    }
                }

                XmlElement itemElement = doc.CreateElement("Item");
                itemElement.InnerText = dir;

                rootNode.PrependChild(itemElement);

                doc.Save(path);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public List<string> GetExportDirHistoryList()
        {
            try
            {
                var ret = new List<string>();

                XmlDocument doc = new XmlDocument();
                var path = AppPathConfig.RecentExportPathsXml;
                doc.Load(path);
                var rootNode = doc.DocumentElement;
                var items = rootNode.SelectNodes("Item");
                foreach (XmlElement item in items)
                {
                    ret.Add(item.InnerText);
                }

                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }

        
    }
}
