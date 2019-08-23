using Explore.Core.Configuration;
using Explore.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Infrastructure
{
    /// <summary>
    /// 实现此接口的类可以用作构成引擎的各种服务。
    /// 通过这个接口编辑功能，模块和实现通过这个访问大多数功能。
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Container manager
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// 初始化组件和插件在环境中
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(NopConfig config);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        // <summary>
        //  Resolve dependency
        // </summary>
        // <param name="type">Type</param>
        // <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 解析所有依赖
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();
    }
}
