namespace TestSample.DesignPatterns;

/// <summary>
///     多线程方式使用单例
/// </summary>
internal class SingletonWithLock
{
    private static SingletonWithLock uniqueInstance;

    private static readonly object locker = new();

    private SingletonWithLock()
    {
    }

    public static SingletonWithLock GetInstance()
    {
        if (uniqueInstance != null) return uniqueInstance;

        lock (locker)
        {
            if (uniqueInstance == null) uniqueInstance = new SingletonWithLock();
        }

        return uniqueInstance;
    }
}