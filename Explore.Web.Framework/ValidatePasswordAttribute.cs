using Explore.Core;
using Explore.Core.Data;
using Explore.Core.Infrastructure;
using Explore.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework
{
    /// <summary>
    /// 表示用于验证用户密码过期的筛选器属性
    /// </summary>
    public class ValidatePasswordAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在操作方法执行之前由ASP.NET MVC框架调用
        /// </summary>
        /// <param name="filterContext">筛选器上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //不将筛选器应用于子方法
            if (filterContext.IsChildAction)
                return;

            var actionName = filterContext.ActionDescriptor.ActionName;
            if (string.IsNullOrEmpty(actionName) || actionName.Equals("ChangePassword", StringComparison.InvariantCultureIgnoreCase))
                return;

            var controllerName = filterContext.Controller.ToString();
            if (string.IsNullOrEmpty(controllerName) || controllerName.Equals("Customer", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //获取当前用户
            var customer = EngineContext.Current.Resolve<IWorkContext>().CurrentCustomer;

            //检查密码过期
            if (customer.PasswordIsExpired())
            {
                var changePasswordUrl = new UrlHelper(filterContext.RequestContext).RouteUrl("CustomerChangePassword");
                filterContext.Result = new RedirectResult(changePasswordUrl);
            }
        }
    }
}
