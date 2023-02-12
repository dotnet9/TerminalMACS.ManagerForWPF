using RPA.Interfaces.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Learn.Activities.Python
{
    public class PythonPrintRedirectObject
    {
        public static PythonPrintRedirectObject Instance = null;

        static PythonPrintRedirectObject()
        {
            Instance = new PythonPrintRedirectObject();
        }

        public void write(string str)
        {
            if (str != "\n")
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Trace, str);
            }
        }
    }
}
