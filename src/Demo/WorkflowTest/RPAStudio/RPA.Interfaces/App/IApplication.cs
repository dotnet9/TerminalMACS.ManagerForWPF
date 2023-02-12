using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Interfaces.App
{
    public interface IApplication
    {
        void Start(string[] args);
        void Shutdown();

        Window OpenMainWindow();
        void OnException();
    }
}
