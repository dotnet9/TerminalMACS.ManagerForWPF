using System.Windows;
using System.Windows.Controls;

namespace WpfAppForZoomInAndZoomOut.Views
{
    internal class TextBoxEx:TextBox
    {
        public new void Undo()
        {
            MessageBox.Show("Undo");
        }

        public new void Redo()
        {
            MessageBox.Show("Redo");
        }
    }
}
