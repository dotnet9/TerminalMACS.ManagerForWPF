using RPA.Interfaces.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPAExecutor.Executor
{
    public interface IWorkflowManager
    {
        void Run();
        void Stop();

        void OnUnhandledException(string title, Exception err);


        string GetConfig(string key);
        void SetConfig(string key, string val);

        void RedirectLogToOutputWindow(SharedObject.enOutputType type, string msg, string msgDetails);
        void RedirectNotification(string notification, string notificationDetails);
    }
}
