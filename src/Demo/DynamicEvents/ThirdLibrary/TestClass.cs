namespace ThirdLibrary;

public class TestClass
{
    /// <summary>
    /// 无参委托
    /// </summary>
    public Action? NoParamEvent;

    /// <summary>
    /// 带1个string参数
    /// </summary>
    public Action<string>? OneParamEvent;

    /// <summary>
    /// 带1个基本类型和自定义类型的委托
    /// </summary>
    public Action<string, EventParam>? TwoParamEvent;

    /// <summary>
    /// EventHandler事件
    /// </summary>
    public static event EventHandler<EventParam>? EventHandlerEvent;

    /// <summary>
    /// 该方法用于触发事件，方便测试
    /// </summary>
    public void CallEvent()
    {
        NoParamEvent?.Invoke();
        OneParamEvent?.Invoke("单参数委托");
        TwoParamEvent?.Invoke("2个参数委托调用成功", new EventParam() { Message = "帅哥，你成功调用啦！" });
        EventHandlerEvent?.Invoke(this, new EventParam { Message = "EventHandler事件调用成功" });
    }
}

/// <summary>
/// 自定义类型，注册时需要使用dynamic接收
/// </summary>
public class EventParam
{
    public string? Message { get; set; }
}