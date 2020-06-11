using System.Windows;
using TerminalMACS.TestDemo.Views.FoodAppLoginUI;

namespace TerminalMACS.TestDemo.Views
{
    public partial class MainTabItem
    {
        public MainTabItem()
        {
            InitializeComponent();
        }

        private void ShowFoodLoginUI_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var view= new FoodAppLoginView();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }
    }
}
