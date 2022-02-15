namespace ObserverModel._3;

// 通知者接口
internal interface Subject
{
    string SubjectState { get; set; }

    void Attach(Observer observer);
    void Detach(Observer observer);
    void Notify();
}