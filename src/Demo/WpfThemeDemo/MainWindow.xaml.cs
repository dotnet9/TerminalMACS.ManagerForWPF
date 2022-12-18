using System.Windows;

namespace WpfThemeDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ChangeTheme_Click(object sender, RoutedEventArgs e)
    {
        ResourceDictionary resource = new();
        if (Application.Current.Resources.MergedDictionaries[0].Source.ToString() ==
            "pack://application:,,,/WpfThemeDemo;component/Resources/Light.xaml")
        {
            resource.Source = new Uri("pack://application:,,,/WpfThemeDemo;component/Resources/Dark.xaml");
        }
        else
        {
            resource.Source = new Uri("pack://application:,,,/WpfThemeDemo;component/Resources/Light.xaml");
        }

        Application.Current.Resources.MergedDictionaries[0] = resource;
    }
}