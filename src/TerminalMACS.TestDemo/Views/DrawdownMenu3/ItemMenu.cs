using System.Collections.Generic;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace TerminalMACS.TestDemo.Views.DrawdownMenu3
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind icon, UserControl screen = null)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
            Screen = screen;
        }

        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}