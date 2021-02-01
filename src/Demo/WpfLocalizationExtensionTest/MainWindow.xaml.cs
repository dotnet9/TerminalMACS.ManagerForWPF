using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFLocalizeExtension.Engine;

namespace WpfLocalizationExtensionTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ButtonGet_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(LangHelper.GetLocalizedString("Test"));
		}
		bool isEn = true;
		private void ChangeLanguge_Click(object sender, RoutedEventArgs e)
		{
			string lan = isEn ? "en-US" : "zh-CN";
			isEn = !isEn;
			WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
			LocalizeDictionary.Instance.Culture =
			WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = System.Globalization.CultureInfo.GetCultureInfo(lan);
		}
	}
}
