using System.Windows;

namespace TerminalMACS.TestDemo.Views.ReDesignInstagram
{
    /// <summary>
    /// Interaction logic for ReDesignInstagramView.xaml
    /// </summary>
    public partial class ReDesignInstagramView : Window
    {
        public ReDesignInstagramView()
        {
            InitializeComponent();
        }
        private void textSearch_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
                textSearch.Visibility = Visibility.Collapsed;
            else
                textSearch.Visibility = Visibility.Visible;
        }
    }
}
