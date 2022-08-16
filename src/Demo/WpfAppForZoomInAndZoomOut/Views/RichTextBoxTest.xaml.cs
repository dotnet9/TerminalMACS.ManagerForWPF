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
using System.Windows.Shapes;

namespace WpfAppForZoomInAndZoomOut
{
    /// <summary>
    /// Interaction logic for RichTextBoxTest.xaml
    /// </summary>
    public partial class RichTextBoxTest : Window
    {
        public RichTextBoxTest()
        {
            InitializeComponent();
            this.TextBox.Text = "•";
        }

        private void AddUnorderedList_Click(object sender, RoutedEventArgs e)
        {
            this.List.MarkerStyle = this.List.MarkerStyle == TextMarkerStyle.Disc
                ? TextMarkerStyle.None
                : TextMarkerStyle.Disc;
        }

        private void AddOrderedList_Click(object sender, RoutedEventArgs e)
        {
            this.List.MarkerStyle = this.List.MarkerStyle == TextMarkerStyle.Decimal
                ? TextMarkerStyle.None
                : TextMarkerStyle.Decimal;
        }
    }
}