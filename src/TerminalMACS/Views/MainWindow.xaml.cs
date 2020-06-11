using AduSkin.Controls.Metro;
using System.ComponentModel;
using System.Windows;
using TerminalMACS.Infrastructure.UI;
using WpfExtensions.Xaml;

namespace TerminalMACS.Views
{
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            this.Closed += delegate { Application.Current.Shutdown(); };
            Theme.ColorChange += delegate
            {
                // Do not bind colors through XAML, unable to get notifications
                BorderBrush = Theme.CurrentColor.OpaqueSolidColorBrush;
            };
        }


        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            string language = (sender as MetroMenuItem).Tag.ToString();
            LanguageHelper.SetLanguage(language);
        }

        private void ShowAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutView = new About();
            aboutView.Owner = this;
            aboutView.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void ShowLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Owner = this;
            login.ShowDialog();
        }
    }
}
