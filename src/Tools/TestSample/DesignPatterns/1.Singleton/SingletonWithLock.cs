using System;
using System.Collections.Generic;
using System.Text;

namespace TestSample.DesignPatterns
{
    /// <summary>
    /// 多线程方式使用单例
    /// </summary>
    class SingletonWithLock
    {
        private static SingletonWithLock uniqueInstance;

        private static readonly object locker = new object();

        private SingletonWithLock()
        {

        }

        public static SingletonWithLock GetInstance()
        {
            if (uniqueInstance != null)
            {
                return uniqueInstance;
            }

            lock (locker)
            {
                if (uniqueInstance == null)
                {
                    uniqueInstance = new SingletonWithLock();
                }
            }

            return uniqueInstance;
        }
    }
}
