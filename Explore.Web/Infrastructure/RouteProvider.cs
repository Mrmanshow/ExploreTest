using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Explore.Web.Framework.Localization;

namespace Explore.Web.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //index
            routes.MapLocalizedRoute("HomePage",
                            "index/",
                            new { controller = "Home", action = "Index" },
                            new[] { "Explore.Web.Controllers" });

            //login
            routes.MapLocalizedRoute("Login",
                            "login/",
                            new { controller = "User", action = "Login" },
                            new[] { "Explore.Web.Controllers" });

            //register
            routes.MapLocalizedRoute("Register",
                            "register/",
                            new { controller = "User", action = "Register" },
                            new[] { "Explore.Web.Controllers" });

            //注册结果页
            routes.MapLocalizedRoute("RegisterResult",
                            "registerresult/{resultId}",
                            new { controller = "User", action = "RegisterResult" },
                            new { resultId = @"\d+" },
                            new[] { "Explore.Web.Controllers" });
        }


        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}