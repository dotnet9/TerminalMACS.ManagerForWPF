using RPA.Shared.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Project
{
    public interface IProjectConfigFileService
    {
        string CurrentProjectJsonFilePath { get; set; }

        ProjectJsonConfig ProjectJsonConfig { get; set; }

        bool Load(string projectConfigFilePath);

        bool Save();
    }
}
