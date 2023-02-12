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

namespace RPA.Shared.UserControls
{
    /// <summary>
    /// SearchTextBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class SearchTextBoxControl : UserControl
    {
        public SearchTextBoxControl()
        {
            InitializeComponent();
        }

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HintText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(SearchTextBoxControl), new PropertyMetadata("请输入搜索内容"));




        public string ClearToolTipText
        {
            get { return (string)GetValue(ClearToolTipTextProperty); }
            set { SetValue(ClearToolTipTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClearToolTipText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearToolTipTextProperty =
            DependencyProperty.Register("ClearToolTipText", typeof(string), typeof(SearchTextBoxControl), new PropertyMetadata("清除搜索内容"));




        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchTextBoxControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ICommand SearchClearCommand
        {
            get { return (ICommand)GetValue(SearchClearCommandProperty); }
            set { SetValue(SearchClearCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchClearCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchClearCommandProperty =
            DependencyProperty.Register("SearchClearCommand", typeof(ICommand), typeof(SearchTextBoxControl), new PropertyMetadata(null));



        private void SearchClearBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchText = "";
            if (SearchClearCommand != null)
            {
                SearchClearCommand.Execute(null);
            }
        }

        private void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchText == "")
            {
                searchClearBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                searchClearBtn.Visibility = Visibility.Visible;
            }
        }

    }
}
