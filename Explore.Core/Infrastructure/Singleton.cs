using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Infrastructure
{
    /// <summary>
    /// 静态编译的“singleton”存在于应用程序域的整个生命周期。
    /// 作为一种标准化的方式来存储单例。
    /// </summary>
    /// <typeparam name="T">要存储的对象类型</typeparam>
    /// <remarks>对实例的访问不同步</remarks>
    public class Singleton<T> : Singleton
    {
        static T instance;

        /// <summary>每次只能指定一个单例类型</summary>
        public static T Instance
        {
            get { return instance; }
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }


    /// <summary>
    /// 所有单例存储在"singletons"<see cref="Singleton{T}"/>.
    /// </summary>
    public class Singleton
    {
        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        static readonly IDictionary<Type, object> allSingletons;

        /// <summary>类型为singleton实例的字典。</summary>
        public static IDictionary<Type, object> AllSingletons
        {
            get { return allSingletons; }
        }
    }
}
