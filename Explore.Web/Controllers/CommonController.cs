using Explore.Core;
using Explore.Core.Caching;
using Explore.Core.Infrastructure;
using Explore.Services.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Controllers
{
    public partial class CommonController : BaseAdminController
    {
        #region Fields

        private readonly IWebHelper _webHelper;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;

        #endregion

        #region Constructors

        public CommonController(HttpContextBase httpContext,
            IWebHelper webHelper,
            IPermissionService permissionService)
        {
            this._permissionService = permissionService;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
        }

        #endregion

        #region Utitlies

        private bool IsDebugAssembly(Assembly assembly)
        {
            var attribs = assembly.GetCustomAttributes(typeof(System.Diagnostics.DebuggableAttribute), false);

            if (attribs.Length > 0)
            {
                var attr = attribs[0] as System.Diagnostics.DebuggableAttribute;
                if (attr != null)
                {
                    return attr.IsJITOptimizerDisabled;
                }
            }

            return false;
        }

        private DateTime GetBuildDate(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;

            const int cPeHeaderOffset = 60;
            const int cLinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, 2048);
            }

            var offset = BitConverter.ToInt32(buffer, cPeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + cLinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        #endregion

        #region Methods

        [HttpPost]
        public virtual ActionResult ClearCache(string returnUrl = "")
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAdministrator))
                return AccessDeniedView();

            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
            cacheManager.Clear();

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            return Redirect(returnUrl);
        }

        [HttpPost]
        public virtual ActionResult RestartApplication(string returnUrl = "")
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAdministrator))
                return AccessDeniedView();

            //重启应用
            _webHelper.RestartAppDomain();

            //主页
            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            //防止开放式重定向攻击
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            return Redirect(returnUrl);
        }    

        #endregion
    }
}