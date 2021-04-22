using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleAppForDotnet6
{
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

  class Program
  {
    //static void Main(string[] args)
    //{
    //  IBase aa = new ChildBB();
    //  aa.Print();
    //}
  }

  /// <summary>
  /// 要求所有的迭代器全部实现该接口
  /// </summary>
  interface IMyEnumerator
  {
    bool MoveNext();
    object Current { get; }
  }

  /// <summary>
  /// 要求所有的集合实现该接口
  /// 这样一来，客户端就可以针对该接口编码
  /// 而无需关注具体的实现
  /// </summary>
  interface IMyEnumerable
  {
    IMyEnumerator GetEnumerator();
    int Count { get; }
  }

  class MyList : IMyEnumerable
  {
    object[] items = new object[10];
    IMyEnumerator myEnumerator;

    public object this[int i]
    {
      get { return items[i]; }
      set { this.items[i] = value; }
    }

    public int Count
    {
      get { return items.Length; }
    }

    public IMyEnumerator GetEnumerator()
    {
      if (myEnumerator == null)
      {
        myEnumerator = new MyEnumerator(this);
      }
      return myEnumerator;

    }
  }

  class MyEnumerator : IMyEnumerator
  {
    int index = 0;
    MyList myList;

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
      else
      {
        index++;
        return true;
      }
    }

    public object Current
    {
      get { return myList[index - 1]; }
    }
  }

}

