using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleAppForDotnet6;

internal class TestArray
{
    private static int collectionCount;
    private static Stopwatch watch;
    private static readonly int testCount = 10000000;

    private static void Main(string[] args)
    {
        Console.WriteLine("开始测试ArrayList : ");
        TestBegin();
        TestArrayList();
        TestEnd();
        Console.WriteLine("开始测试List<T>:");
        TestBegin();
        TestGenericList();
        TestEnd();
    }

    private static void TestBegin()
    {
        GC.Collect(); //强制对所有代码进行即时垃圾回收
        GC.WaitForPendingFinalizers(); //挂起线程，执行终结器队列中的终结器(即析构方法）
        GC.Collect(); //再次对所有代码进行垃圾回收，主要包括从终结器队列中出来的对象
        collectionCount = GC.CollectionCount(0); //返回在0代中执行的垃圾回收次数
        watch = new Stopwatch();
        watch.Start();
    }

    private static void TestEnd()
    {
        watch.Stop();
        Console.WriteLine("耗时:" + watch.ElapsedMilliseconds);
        Console.WriteLine("垃圾回收次数: " + (GC.CollectionCount(0) - collectionCount));
    }

    private static void TestArrayList()
    {
        var al = new ArrayList();
        var temp = 0;
        for (var i = 0; i < testCount; i++)
        {
            al.Add(i);
            temp = (int)al[i];
        }

        al = null;
    }

    private static void TestGenericList()
    {
        var listT = new List<int>();
        var temp = 0;
        for (var i = 0; i < testCount; i++)
        {
            listT.Add(i);
            temp = listT[i];
        }

        listT = null;
    }
}