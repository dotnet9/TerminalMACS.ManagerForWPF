using RPA.Interfaces.Project;
using RPA.Shared.Configs;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RPA.Services.Project
{
    public class RecentProjectsConfigService : IRecentProjectsConfigService
    {
        private IProjectConfigFileService _projectConfigFileService;

        public event EventHandler ChangeEvent;

        public RecentProjectsConfigService(IProjectConfigFileService projectConfigFileService)
        {
            _projectConfigFileService = projectConfigFileService;
        }

        public void Add(string projectConfigFilePath)
        {
            _projectConfigFileService.Load(projectConfigFilePath);

            var projectName = _projectConfigFileService.ProjectJsonConfig.name;
            var projectDescription = _projectConfigFileService.ProjectJsonConfig.description;

            Add(projectConfigFilePath, projectName, projectDescription);

            ChangeEvent?.Invoke(this, EventArgs.Empty);
        }

        private bool Add(string projectConfigFilePath, string projectName, string projectDescription)
        {
            XmlDocument doc = new XmlDocument();
            var path = AppPathConfig.RecentProjectsXml;
            doc.Load(path);
            var rootNode = doc.DocumentElement;

            var projectNodes = rootNode.SelectNodes("Project");
            //最多记录N条，默认显示个数由XML的MaxShowCount限制
            if (projectNodes.Count > ProjectConstantConfig.RecentProjectsMaxRecordCount)
            {
                rootNode.RemoveChild(rootNode.LastChild);
            }

            foreach (XmlElement item in projectNodes)
            {
                var filePath = item.GetAttribute("FilePath");
                if (projectConfigFilePath == filePath)
                {
                    rootNode.RemoveChild(item);
                    break;
                }
            }

            XmlElement projectElement = doc.CreateElement("Project");
            projectElement.SetAttribute("FilePath", projectConfigFilePath);
            projectElement.SetAttribute("Name", projectName);
            projectElement.SetAttribute("Description", projectDescription);

            rootNode.PrependChild(projectElement);

            doc.Save(path);

            return true;
        }

        public void Remove(string projectConfigFilePath)
        {
            XmlDocument doc = new XmlDocument();
            var path = AppPathConfig.RecentProjectsXml;
            doc.Load(path);
            var rootNode = doc.DocumentElement;

            var projectNodes = rootNode.SelectNodes("Project");

            foreach (XmlElement item in projectNodes)
            {
                var filePath = item.GetAttribute("FilePath");
                if (projectConfigFilePath == filePath)
                {
                    rootNode.RemoveChild(item);
                    break;
                }
            }

            doc.Save(path);

            ChangeEvent?.Invoke(this, EventArgs.Empty);
        }

        public List<object> Load()
        {
            var list = new List<object>();

            XmlDocument doc = new XmlDocument();
            var path = AppPathConfig.RecentProjectsXml;
            doc.Load(path);
            var rootNode = doc.DocumentElement;

            var maxShowCount = ProjectConstantConfig.RecentProjectsMaxRecordCount;
            var projectNodes = rootNode.SelectNodes("Project");
            foreach (XmlNode dir in projectNodes)
            {
                var filePath = (dir as XmlElement).GetAttribute("FilePath");
                var name = (dir as XmlElement).GetAttribute("Name");
                var description = (dir as XmlElement).GetAttribute("Description");

                dynamic item = new ExpandoObject();
                item.FilePath = filePath;
                item.Name = name;
                item.Description = description;

                list.Add(item);

                maxShowCount--;
                if (maxShowCount == 0)
                {
                    break;
                }
            }

            return list;
        }

        public void Update(string projectConfigFilePath, string name, string description)
        {
            XmlDocument doc = new XmlDocument();
            var path = AppPathConfig.RecentProjectsXml;
            doc.Load(path);
            var rootNode = doc.DocumentElement;

            var projectNodes = rootNode.SelectNodes("Project");

            foreach (XmlElement item in projectNodes)
            {
                var filePath = item.GetAttribute("FilePath");
                if (projectConfigFilePath == filePath)
                {
                    item.SetAttribute("Name", name);
                    item.SetAttribute("Description", description);
                    break;
                }
            }

            doc.Save(path);

            ChangeEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
