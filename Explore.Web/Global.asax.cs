using Explore.Core.Infrastructure;
using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Explore.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //设置传输协议版本
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //移除 "X-AspNetMvc-Version" 响应头
            MvcHandler.DisableMvcResponseHeader = true;

            //初始化引擎上下文
            EngineContext.Initialize(false);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //在默认ModelMetadataProvider上添加一些功能
            ModelMetadataProviders.Current = new ExploreMetadataProvider();

            //fluent validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new ExloreValidatorFactory())); 
        }
    }
}
