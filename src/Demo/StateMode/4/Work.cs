namespace StateMode._4;

public class Work
{
    /// <summary>
    ///     工作初始化为上午工作状态，即上午9点开始上班
    /// </summary>
    public Work()
    {
        current = new ForenoonState();
    }

    public State current { get; set; }

    public int Hour { get; set; }

    public bool TaskFinished { get; set; }

    public void SetState(State state)
    {
        current = state;
    }

    public void WriteProgram()
    {
        current.WriteProgram(this);
    }
}