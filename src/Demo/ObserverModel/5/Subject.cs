namespace ObserverModel._5;

/// <summary>
///     通知者接口
/// </summary>
internal interface Subject
{
    string SubjectState { get; set; }
    void Notify();
}