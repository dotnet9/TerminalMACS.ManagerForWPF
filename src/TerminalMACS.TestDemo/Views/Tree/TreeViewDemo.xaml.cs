using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace TerminalMACS.TestDemo.Views.Tree;

/// <summary>
///     TreeViewDemo.xaml 的交互逻辑
/// </summary>
public partial class TreeViewDemo : Window
{
    public TreeViewDemo()
    {
        InitializeComponent();

        string[] icons =
        {
            "../Images/Avatar.png",
            "../Images/Basket.png",
            "../Images/Bell.png",
            "../Images/Box.png",
            "../Images/Camera.png",
            "../Images/Delivery.png",
            "../Images/Phone.png"
        };
        var rd = new Random(DateTime.Now.Millisecond);

        tvLeft.ItemsSource = new ObservableCollection<CusMenuInfo>
        {
            new(1, "Node1", icons[0]),
            new(1, "Node2", icons[1]),
            new(1, "Node3", icons[2])
            {
                Children = new ObservableCollection<CusMenuInfo>
                {
                    new(2, "Node31", icons[5]),
                    new(2, "Node32", icons[6]),
                    new(2, "Node33", icons[0])
                }
            },
            new(1, "Node4", icons[3])
            {
                Children = new ObservableCollection<CusMenuInfo>
                {
                    new(2, "Node41", icons[1]),
                    new(2, "Node42", icons[2])
                    {
                        Children = new ObservableCollection<CusMenuInfo>
                        {
                            new(3, "Node421", icons[3]),
                            new(3, "Node422", icons[4]),
                            new(3, "Node423", icons[5])
                        }
                    },
                    new(2, "Node43", icons[6])
                }
            },
            new(1, "Node5", icons[4])
        };
    }
}