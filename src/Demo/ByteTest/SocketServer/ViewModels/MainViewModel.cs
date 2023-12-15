namespace SocketServer.ViewModels;

public class MainViewModel : BindableBase
{
    private string _tcpIp = "127.0.0.1";

    public string TcpIp
    {
        get => _tcpIp;
        set => SetProperty(ref _tcpIp, value);
    }

    private int _tcpPort = 5000;

    public int TcpPort
    {
        get => _tcpPort;
        set => SetProperty(ref _tcpPort, value);
    }
}