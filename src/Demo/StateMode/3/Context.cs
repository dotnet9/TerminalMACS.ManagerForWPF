using System;

namespace StateMode._3;

internal class Context
{
    private State state;

    /// <summary>
    ///     定义Context的初始状态
    /// </summary>
    /// <param name="state"></param>
    public Context(State state)
    {
        State = state;
    }

    public State State
    {
        get => state;
        set
        {
            state = value;
            Console.WriteLine($"当前状态：{state.GetType().Name}");
        }
    }

    /// <summary>
    ///     对请求做处理，并设置下一状态
    /// </summary>
    public void Request()
    {
        State.Handle(this);
    }
}