using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalMACS.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "test";
        public string Title
        {
            get { return _title; }
            set
            {
                if(value!=_title)
                {
                    SetProperty(ref _title, value);
                }
            }
        }
    }
}
