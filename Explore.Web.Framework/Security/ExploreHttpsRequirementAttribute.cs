using Explore.Core;
using Explore.Core.Data;
using Explore.Core.Domain.Security;
using Explore.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ExploreHttpsRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        public ExploreHttpsRequirementAttribute(SslRequirement sslRequirement)
        {
            this.SslRequirement = sslRequirement;
        }
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            //不将筛选器应用于子方法
            if (filterContext.IsChildAction)
                return;

            // 只重定向GET请求，
            // 否则，浏览器可能无法正确传播动词和请求正文。
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;
            var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();
            if (securitySettings.ForceSslForAllPages)
                //无论指定值如何，所有页面都强制为SSL。
                this.SslRequirement = SslRequirement.Yes;

            switch (this.SslRequirement)
            {
                case SslRequirement.Yes:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (!currentConnectionSecured)
                        {
                            //重定向到网页的HTTPS版本
                            string url = webHelper.GetThisPageUrl(true, true);

                            //301（永久）重定向
                            filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    break;
                case SslRequirement.No:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (currentConnectionSecured)
                        {
                            //重定向到网页的HTTP版本
                            string url = webHelper.GetThisPageUrl(true, false);
                            //301（永久）重定向
                            filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    break;
                case SslRequirement.NoMatter:
                    {
                        //do nothing
                    }
                    break;
                default:
                    throw new ExploreException("Not supported SslProtected parameter");
            }
        }

        public SslRequirement SslRequirement { get; set; }
    }
}
