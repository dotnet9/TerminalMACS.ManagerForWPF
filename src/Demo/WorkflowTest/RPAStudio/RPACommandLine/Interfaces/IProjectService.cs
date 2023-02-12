using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPACommandLine.Interfaces
{
    public interface IProjectService
    {
        string ProjectDirectory { get; }

        string ProjectJsonFile { get; }

        string XamlFile { get; }

        string Name { get; }

        string Version { get; }

        void Init(string filePath);

        void Start();

        void Stop();
    }
}
