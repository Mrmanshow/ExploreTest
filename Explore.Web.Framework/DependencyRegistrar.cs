using Autofac;
using Explore.Core.Configuration;
using Explore.Core.Infrastructure;
using Explore.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac.Integration.Mvc;
using Explore.Data;
using Explore.Core.Data;
using Explore.Core.Caching;
using Explore.Core.Fakes;
using Explore.Services.Logging;
using Explore.Web.Framework.UI;
using System.Reflection;
using Autofac.Core;
using Autofac.Builder;
using Explore.Core;
using Explore.Services.Helpers;
using Explore.Services.Security;
using Explore.Services.Customers;
using Explore.Services.Authentication;
using Explore.Services.Configuration;
using Explore.Web.Framework.MVC.Routes;
using Explore.Services.Common;
using Explore.Services.Localization;
using Explore.Services.Events;
using Explore.Services.Cms;
using Explore.Core.Plugins;
using Explore.Services.Users;
using Explore.Services.Content;
using Explore.Services.Media;

namespace Explore.Web.Framework
{
    /// <summary>
    /// 依赖注册
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// 注册服务和接口
        /// </summary>
        /// <param name="builder">注册容器</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">配置对象</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //HTTP上下文和其他相关内容
            builder.Register(c =>
                //当HttpContext不可用时注册FakeHttpContext
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //注册控制器
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //数据层
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();


            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();

                builder.Register<IDbContext>(c => new ExploreObjectContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
            }
            else
            {
                builder.Register<IDbContext>(c => new ExploreObjectContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();
            }


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();


            //缓存管理
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_static").InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_static").SingleInstance();
            }
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_per_request").InstancePerLifetimeScope();

            //services
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<UsersService>().As<IUsersService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRegistrationService>().As<ICustomerRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<BannerService>().As<IBannerService>().InstancePerLifetimeScope();

            builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();



            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();

            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();


            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();

            //use static cache (between HTTP requests)
            builder.RegisterType<PermissionService>().As<IPermissionService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerLifetimeScope();

            //use static cache (between HTTP requests)
            builder.RegisterType<SettingService>().As<ISettingService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();
            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();

            //use static cache (between HTTP requests)
            builder.RegisterType<LocalizationService>().As<ILocalizationService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerLifetimeScope();

            //use static cache (between HTTP requests)
            builder.RegisterType<CustomerActivityService>().As<ICustomerActivityService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerLifetimeScope();

            builder.RegisterType<WidgetService>().As<IWidgetService>().InstancePerLifetimeScope();

            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();

            //注册事件消费者
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
        }

        /// <summary>
        /// 依赖注册的顺序
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }

    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    //var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
                    //uncomment the code below if you want load settings per store only when you have two stores installed.
                    //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
                    //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

                    //although it's better to connect to your database and execute the following SQL:
                    //DELETE FROM [Setting] WHERE [StoreId] > 0
                    return c.Resolve<ISettingService>().LoadSetting<TSettings>(0);
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }
}
