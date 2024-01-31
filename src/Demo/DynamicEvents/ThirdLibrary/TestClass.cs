namespace ThirdLibrary;

public class TestClass
{
    public Action? NoParamEvent;

    public Action<string>? OneParamEvent;
    public Action<string, EventParam>? TwoParamEvent;
    public static event EventHandler<EventParam> EventHandlerEvent;

    public void CallTwoParamEvent()
    {
        TwoParamEvent?.Invoke("恭喜", new EventParam() { Message = "帅哥，你成功调用啦！" });
        EventHandlerEvent?.Invoke(this, new EventParam { Message = "EventHandler类型调用成功"});
    }
}

public class EventParam
{
    public string Message { get; set; }
}