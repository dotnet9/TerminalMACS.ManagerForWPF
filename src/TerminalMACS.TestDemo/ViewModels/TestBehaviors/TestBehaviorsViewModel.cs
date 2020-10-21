using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.ViewModels.TestBehaviors
{
    public class TestBehaviorsViewModel:BindableBase
    {
        private string _CurrentText;
        public string CurrentText
        {
            get { return _CurrentText; }
            set { SetProperty(ref _CurrentText, value); }
        }


        public ICommand ButtonClickCommand { get; private set; }

        public TestBehaviorsViewModel()
        {
            ButtonClickCommand = new DelegateCommand(() => MessageBox.Show("DD"));
        }
    }

}
