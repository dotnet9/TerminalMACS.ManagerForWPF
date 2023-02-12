using RPA.Interfaces.Share;
using RPARobot.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARobot.Interfaces
{
    public interface IRunManagerService
    {
        event EventHandler BeginRunEvent;
        event EventHandler EndRunEvent;

        PackageItemViewModel PackageItem { get; }

        bool HasException { get; }

        void Init(PackageItemViewModel packageItem, string xamlPath, List<string> activitiesDllLoadFrom, List<string> dependentAssemblies);

        void Run();

        void Stop();

        void LogToOutputWindow(SharedObject.enOutputType type, string msg, string msgDetails);
    }
}