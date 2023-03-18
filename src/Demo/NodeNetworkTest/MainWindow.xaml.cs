using DynamicData;
using NodeNetwork.ViewModels;
using System.Windows;

namespace NodeNetworkTest;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // 创建NetworkView视图的ViewModel实例
        var network = new NetworkViewModel();

        // 给视图(networkView)赋值viewmodel(network)
        networkView.ViewModel = network;

        // 创建第一个节点ViewModel，设置它的名称并将此节点加入到network
        var node1 = new NodeViewModel();
        node1.Name = "节点1";
        network.Nodes.Add(node1);

        // 创建第一个节点的输入端口ViewModel，设置它的名称并加入第一个节点
        var node1Input = new NodeInputViewModel();
        node1Input.Name = "节点1输入";
        node1.Inputs.Add(node1Input);

        // 创建第二个节点ViewModel，设置它的名称并将此节点加入到network, 并以同样的方式给此节点添加一个输出Create the second node viewmodel, set its name, add it to the network and add an output in a similar fashion.
        var node2 = new NodeViewModel();
        node2.Name = "节点2";
        node2.Position = new Point(100, 100);
        network.Nodes.Add(node2);

        var node2Output = new NodeOutputViewModel();
        node2Output.Name = "节点2输出";
        node2.Outputs.Add(node2Output);

        // 将节点1的输入端口和节点2的输出端口连接到一起
        var connection = new ConnectionViewModel(network, node1Input, node2Output);
        network.Connections.Add(connection);
    }
}