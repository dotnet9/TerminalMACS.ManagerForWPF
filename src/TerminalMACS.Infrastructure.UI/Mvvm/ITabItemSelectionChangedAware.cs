using System;
using System.Collections.Generic;
using System.Text;

namespace TerminalMACS.Infrastructure.UI.Mvvm
{
    public interface ITabItemSelectionChangedAware
    {
        void OnSelected();

        void OnUnselected();
    }
}
