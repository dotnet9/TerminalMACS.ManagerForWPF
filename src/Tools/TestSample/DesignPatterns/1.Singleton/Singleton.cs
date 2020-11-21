using System;
using System.Collections.Generic;
using System.Text;

namespace TestSample.DesignPatterns
{
    /// <summary>
    /// 单线程使用此种单例没问题，多线程有问题
    /// </summary>
    class Singleton
    {
        private static Singleton uniqueInstance;

        /// <summary>
        /// 私有构造函数，使外界不能创建该类实例
        /// </summary>
        private Singleton()
        {

        }

        /// <summary>
        /// 全局访问点，使用公有属性也可以
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            if(uniqueInstance==null)
            {
                uniqueInstance = new Singleton();
            }

            return uniqueInstance;
        }
    }
}
