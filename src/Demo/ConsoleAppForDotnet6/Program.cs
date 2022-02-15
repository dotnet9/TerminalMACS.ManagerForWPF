using System;

namespace ConsoleAppForDotnet6;

public interface IBase
{
    void Print();
    void PrintB();
}

public abstract class BaseAA : IBase
{
    public void Print()
    {
        PrintB();
    }

    public abstract void PrintB();
}

public class ChildBB : BaseAA
{
    public override void PrintB()
    {
        Console.WriteLine("BB==>PrintB()");
    }
}

internal class Program
{
    //static void Main(string[] args)
    //{
    //  IBase aa = new ChildBB();
    //  aa.Print();
    //}
}

/// <summary>
///     要求所有的迭代器全部实现该接口
/// </summary>
internal interface IMyEnumerator
{
    object Current { get; }
    bool MoveNext();
}

/// <summary>
///     要求所有的集合实现该接口
///     这样一来，客户端就可以针对该接口编码
///     而无需关注具体的实现
/// </summary>
internal interface IMyEnumerable
{
    int Count { get; }
    IMyEnumerator GetEnumerator();
}

internal class MyList : IMyEnumerable
{
    private readonly object[] items = new object[10];
    private IMyEnumerator myEnumerator;

    public object this[int i]
    {
        get => items[i];
        set => items[i] = value;
    }

    public int Count => items.Length;

    public IMyEnumerator GetEnumerator()
    {
        if (myEnumerator == null) myEnumerator = new MyEnumerator(this);
        return myEnumerator;
    }
}

internal class MyEnumerator : IMyEnumerator
{
    private int index;
    private readonly MyList myList;

    public MyEnumerator(MyList myList)
    {
        this.myList = myList;
    }

    public bool MoveNext()
    {
        if (index + 1 > myList.Count)
        {
            index = 1;
            return false;
        }

        index++;
        return true;
    }

    public object Current => myList[index - 1];
}