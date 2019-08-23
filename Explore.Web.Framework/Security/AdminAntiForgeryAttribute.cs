using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAntiForgeryAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _ignore;

        /// <summary>
        /// 防伪安全属性
        /// </summary>
        /// <param name="ignore">传递false以忽略此安全验证</param>
        public AdminAntiForgeryAttribute(bool ignore = false)
        {
            this._ignore = ignore;
        }
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (_ignore)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            //only POST requests
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                return;

            //if (!DataSettingsHelper.DatabaseIsInstalled())
            //    return;
            //var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();
            //if (!securitySettings.EnableXsrfProtectionForAdminArea)
            //    return;

            var validator = new ValidateAntiForgeryTokenAttribute();
            validator.OnAuthorization(filterContext);
        }
    }
}
