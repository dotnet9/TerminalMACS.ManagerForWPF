using System.Windows;

namespace RPATest;

public partial class MainWindow : Window
{
    private enum DesignerType
    {
        Sequence,
        Flowchart,
        StateMachine
    }
    public MainWindow()
    {
        InitializeComponent();
    }
}
