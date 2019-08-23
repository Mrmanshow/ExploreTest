using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Explore.Core.Configuration;
using Explore.Core.Infrastructure.DependencyManagement;
using Explore.Core.Infrastructure.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Core.Infrastructure
{
    public class NopEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Utilities

        /// <summary>
        /// Run startup tasks
        /// </summary>
        //protected virtual void RunStartupTasks()
        //{
        //    var typeFinder = _containerManager.Resolve<ITypeFinder>();
        //    var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
        //    var startUpTasks = new List<IStartupTask>();
        //    foreach (var startUpTaskType in startUpTaskTypes)
        //        startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
        //    //sort
        //    startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
        //    foreach (var startUpTask in startUpTasks)
        //        startUpTask.Execute();
        //}

        /// <summary>
        /// 注册依赖
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterDependencies(NopConfig config)
        {
            var builder = new ContainerBuilder();

            //依赖
            var typeFinder = new WebAppTypeFinder();
            builder.RegisterInstance(config).As<NopConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //注册其他程序集提供的依赖
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            //根据类型实例化
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //注册顺序
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder, config);

            var container = builder.Build();
            this._containerManager = new ContainerManager(container);

            //设置依赖解析
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        /// <summary>
        /// 注册映射
        /// </summary>
        /// <param name="config">Config</param>
        protected virtual void RegisterMapperConfiguration(NopConfig config)
        {
            //依赖
            var typeFinder = new WebAppTypeFinder();

            //由其他程序集提供的映射器配置
            var mcTypes = typeFinder.FindClassesOfType<IMapperConfiguration>();
            var mcInstances = new List<IMapperConfiguration>();
            foreach (var mcType in mcTypes)
                mcInstances.Add((IMapperConfiguration)Activator.CreateInstance(mcType));
            //排序
            mcInstances = mcInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //获取配置
            var configurationActions = new List<Action<IMapperConfigurationExpression>>();
            foreach (var mc in mcInstances)
                configurationActions.Add(mc.GetConfiguration());
            //注册
            AutoMapperConfiguration.Init(configurationActions);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化组件和插件在环境中
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(NopConfig config)
        {
            //注册依赖
            RegisterDependencies(config);

            //注册映射配置
            RegisterMapperConfiguration(config);

            //startup tasks
            //if (!config.IgnoreStartupTasks)
            //{
            //    RunStartupTasks();
            //}

        }

        /// <summary>
        /// 解析依赖
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        ///  解析依赖
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// 解析全部依赖
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 依赖注入容器
        /// </summary>
        public virtual ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }
}
