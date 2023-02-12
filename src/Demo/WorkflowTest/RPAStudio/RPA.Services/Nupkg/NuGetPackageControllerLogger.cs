using NuGet.Common;
using RPA.Interfaces.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Nupkg.Nupkg
{
    public class NuGetPackageControllerLogger : ILogger
    {
        public void _Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Verbose:
                case LogLevel.Information:
                case LogLevel.Minimal:
                    SharedObject.Instance.Output(SharedObject.enOutputType.Trace, message);
                    break;
                case LogLevel.Warning:
                    SharedObject.Instance.Output(SharedObject.enOutputType.Warning, message);
                    break;
                case LogLevel.Error:
                    SharedObject.Instance.Output(SharedObject.enOutputType.Error, message);
                    break;
                default:
                    break;
            }
        }
        private static NuGetPackageControllerLogger _instance = null;
        public static NuGetPackageControllerLogger Instance
        {
            get
            {
                if (_instance == null) _instance = new NuGetPackageControllerLogger();
                return _instance;
            }
        }
        public void Log(LogLevel level, string data)
        {
            _Log(level, data);
        }
        public void Log(ILogMessage message)
        {
            _Log(message.Level, message.Message);
        }
        public Task LogAsync(LogLevel level, string data)
        {
            _Log(level, data);
            return Task.FromResult(0);
        }
        public Task LogAsync(ILogMessage message)
        {
            _Log(message.Level, message.Message);
            return Task.FromResult(0);
        }
        public void LogDebug(string data)
        {
            _Log(LogLevel.Debug, data);
        }
        public void LogError(string data)
        {
            _Log(LogLevel.Error, data);
        }
        public void LogInformation(string data)
        {
            _Log(LogLevel.Information, data);
        }
        public void LogInformationSummary(string data)
        {
            _Log(LogLevel.Information, data);
        }
        public void LogMinimal(string data)
        {
            _Log(LogLevel.Minimal, data);
        }
        public void LogVerbose(string data)
        {
            _Log(LogLevel.Verbose, data);
        }
        public void LogWarning(string data)
        {
            _Log(LogLevel.Warning, data);
        }
    }
}
