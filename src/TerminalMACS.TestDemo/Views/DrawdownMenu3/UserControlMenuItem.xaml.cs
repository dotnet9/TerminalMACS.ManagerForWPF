using System.Windows.Controls;

namespace TerminalMACS.TestDemo.Views.DrawdownMenu3
{
    /// <summary>
    /// UserControlMenuItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlMenuItem : UserControl
    {
        DrawdownMenu3Window _context;

        public UserControlMenuItem(ItemMenu itemMenu, DrawdownMenu3Window context)
        {
            InitializeComponent();

            _context = context;

            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;

            this.DataContext = itemMenu;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _context.SwitchScreen(((SubItem)((ListView)sender).SelectedItem).Screen);
        }

        private void ExpanderMenu_OnExpanded(object sender, RoutedEventArgs e)
        {
            _context.SwitchScreen(((ItemMenu)this.DataContext).Screen);
        }
    }
}