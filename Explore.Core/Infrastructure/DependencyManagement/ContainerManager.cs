using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Explore.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖容器管理
    /// </summary>
    public class ContainerManager
    {
        private readonly IContainer _container;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">Conainer</param>
        public ContainerManager(IContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// 获取容器
        /// </summary>
        public virtual IContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">检索键</param>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <returns>已解析的服务</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //未指定作用域
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <returns>已解析的服务</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //未指定作用域
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// Resolve all
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">检索键</param>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <returns>已解析的多个服务</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //未指定作用域
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <returns>已解析的服务</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// 解析未注册的服务
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <returns>已解析的服务</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //未指定作用域
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new ExploreException("Unknown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (ExploreException)
                {

                }
            }
            throw new ExploreException("No constructor  was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// 尝试解析服务
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <param name="instance">已解析的服务</param>
        /// <returns>指示服务是否已成功解析的值</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                //未指定作用域
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// 检查某些服务是否已注册（可以解析）
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //未指定作用域
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// 可选解析
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">作用域；传递空值自动解析当前作用域</param>
        /// <returns>已解析的服务</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //未指定作用域
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// 获取当前作用域
        /// </summary>
        /// <returns>作用域</returns>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                //当返回这样的生存期范围时，应该确保一旦使用它就会被释放（例如在调度任务中）
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                //如果RequestLifeTimeScope已被释放（例如，在“application-endRequest”处理程序中或之后请求），
                //我们可以在此处获取异常，但请注意，通常不应该发生这种情况。

                //当返回这样的生存期范围时，应该确保一旦使用它就会被释放（例如在调度任务中）
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}
