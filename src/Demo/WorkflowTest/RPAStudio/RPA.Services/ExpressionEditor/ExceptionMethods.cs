using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPA.Services.ExpressionEditor
{
    public static class ExceptionMethods
    {
        public const int ERROR_SHARING_VIOLATION = -2147024864;

        public static string Trace(this Exception exception, string label = null)
        {
            string arg = string.IsNullOrWhiteSpace(label) ? string.Empty : (label + ": ");
            string text = $"{arg}{exception}, HResult {exception.HResult}";
            System.Diagnostics.Trace.TraceError(text);
            return text;
        }

        public static bool IsFileInUse(this IOException ioEx)
        {
            if (ioEx == null)
            {
                return false;
            }
            return ioEx.HResult == -2147024864;
        }

        public static Exception GetInnermostException(this Exception exception)
        {
            return WalkInnermostException(exception);
        }

        private static Exception WalkInnermostException(Exception root)
        {
            if (root.InnerException == null)
            {
                return root;
            }
            return WalkInnermostException(root.InnerException);
        }

        public static string GetTopmostNonemptyMessage(this Exception exception)
        {
            return WalkInnermostExceptionMessage(exception);
        }

        private static string WalkInnermostExceptionMessage(Exception root)
        {
            if (root == null)
            {
                return string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(root.Message))
            {
                return root.Message;
            }
            return WalkInnermostExceptionMessage(root.InnerException);
        }

        public static bool IsFatal(this Exception exception)
        {
            while (exception != null)
            {
                if ((exception is OutOfMemoryException && !(exception is InsufficientMemoryException)) || exception is ThreadAbortException)
                {
                    return true;
                }
                if (exception is TypeInitializationException || exception is TargetInvocationException)
                {
                    exception = exception.InnerException;
                    continue;
                }
                if (exception is AggregateException)
                {
                    ReadOnlyCollection<Exception> innerExceptions = ((AggregateException)exception).InnerExceptions;
                    foreach (Exception item in innerExceptions)
                    {
                        if (IsFatal(item))
                        {
                            return true;
                        }
                    }
                }
                break;
            }
            return false;
        }
    }
}
