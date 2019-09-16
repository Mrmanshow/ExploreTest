using Autofac;
using Autofac.Core;
using Explore.Core.Caching;
using Explore.Core.Configuration;
using Explore.Core.Infrastructure;
using Explore.Core.Infrastructure.DependencyManagement;
using Explore.Web.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Infrastructure
{
    public class DependencyRegistrar: IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            ////installation localization service
            //builder.RegisterType<InstallationLocalizationService>().As<IInstallationLocalizationService>().InstancePerLifetimeScope();




            ////controllers (we cache some data between HTTP requests)
            //builder.RegisterType<ProductController>()
            //    .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
            //builder.RegisterType<ShoppingCartController>()
            //    .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));




            //工厂（我们在HTTP请求之间缓存表示模型）
            builder.RegisterType<CustomerModelFactory>().As<ICustomerModelFactory>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserModelFactory>().As<IUserModelFactory>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BannerModeIFactory>().As<IBannerModelFactory>()
                .InstancePerLifetimeScope();
            builder.RegisterType<GameStatisticsModelFactory>().As<IGameStatisticsModelFactory>()
                .InstancePerLifetimeScope();
            builder.RegisterType<LabaModelFactory>().As<ILabaModelFactory>()
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// 依赖注册的顺序
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}