namespace AdapterMode._1;

internal class Adapter : Target
{
    /// <summary>
    ///     建立一个私有的Adaptee对象
    /// </summary>
    private readonly Adaptee adaptee = new();

    /// <summary>
    ///     这样就可以把表面上调用Request()方法变成实际调用SpecificRequest()
    /// </summary>
    public override void Request()
    {
        adaptee.SpecificRequest();
    }
}