using Explore.Core;
using Explore.Core.Data;
using Explore.Core.Infrastructure;
using Explore.Services.Localization;
using Explore.Web.Framework.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework.ViewEngines.Razor
{
    /// <summary>
    /// Web试图页
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private ILocalizationService _localizationService;
        private Localizer _localizer;

        /// <summary>
        /// 获取本地化资源
        /// </summary>
        public Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    //null localizer
                    //_localizer = (format, args) => new LocalizedString((args == null || args.Length == 0) ? format : string.Format(format, args));

                    //default localizer
                    _localizer = (format, args) =>
                    {
                        var resFormat = _localizationService.GetResource(format);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }
                        return
                            new LocalizedString((args == null || args.Length == 0)
                                                    ? resFormat
                                                    : string.Format(resFormat, args));
                    };
                }
                return _localizer;
            }
        }
        public override void InitHelpers()
        {
            base.InitHelpers();

            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                _localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            }
        }

        public override string Layout
        {
            get
            {
                var layout = base.Layout;

                if (!string.IsNullOrEmpty(layout))
                {
                    var filename = Path.GetFileNameWithoutExtension(layout);
                    ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

                    if (viewResult.View != null && viewResult.View is RazorView)
                    {
                        layout = (viewResult.View as RazorView).ViewPath;
                    }
                }

                return layout;
            }
            set
            {
                base.Layout = value;
            }
        }

        /// <summary>
        /// Return a value indicating whether the working language and theme support RTL (right-to-left)
        /// </summary>
        /// <returns></returns>
        //public bool ShouldUseRtlTheme()
        //{
        //    var workContext = EngineContext.Current.Resolve<IWorkContext>();
        //    var supportRtl = workContext.WorkingLanguage.Rtl;
        //    if (supportRtl)
        //    {
        //        //ensure that the active theme also supports it
        //        var themeProvider = EngineContext.Current.Resolve<IThemeProvider>();
        //        var themeContext = EngineContext.Current.Resolve<IThemeContext>();
        //        supportRtl = themeProvider.GetThemeConfiguration(themeContext.WorkingThemeName).SupportRtl;
        //    }
        //    return supportRtl;
        //}
    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
