namespace ThirdLibrary;

public class TestClass
{
    public Action NoParamEvent;

    public Action<string> OneParamEvent;
    public Action<string, EventParam> TwoParamEvent;

    public void CallTwoParamEvent()
    {
        TwoParamEvent?.Invoke("恭喜", new EventParam() { Param1 = "帅哥，你成功调用啦！" });
    }
}

public class EventParam
{
    public string Param1 { get; set; }
}