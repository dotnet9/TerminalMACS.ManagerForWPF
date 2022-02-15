using System.Runtime.InteropServices;

namespace TerminalMACS.TestDemo.Views.BaiduMap;

[ComVisible(true)] // 将该类设置为com可访问
public class OprateBasic
{
    private BaiduMapView instance;

    public OprateBasic(BaiduMapView instance)
    {
        this.instance = instance;
    }
}