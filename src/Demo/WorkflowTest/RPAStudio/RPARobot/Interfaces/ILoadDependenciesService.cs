using RPA.Shared.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARobot.Interfaces
{
    public interface ILoadDependenciesService
    {
        string CurrentProjectJsonFile { get; }

        void Init(string projectJsonFile);

        Task LoadDependencies();

        ProjectJsonConfig ReadProjectJsonConfig();

        List<string> CurrentActivitiesDllLoadFrom { get; }

        List<string> CurrentDependentAssemblies { get; }
    }
}
