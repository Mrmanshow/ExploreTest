using Explore.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Explore.Web.Framework.Security.Captcha
{
    public class CaptchaValidatorAttribute : ActionFilterAttribute
    {
        private const string CHALLENGE_FIELD_KEY = "recaptcha_challenge_field";
        private const string RESPONSE_FIELD_KEY = "recaptcha_response_field";
        private const string G_RESPONSE_FIELD_KEY = "g-recaptcha-response";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool valid = false;
            var captchaChallengeValue = filterContext.HttpContext.Request.Form[CHALLENGE_FIELD_KEY];
            var captchaResponseValue = filterContext.HttpContext.Request.Form[RESPONSE_FIELD_KEY];
            var gCaptchaResponseValue = filterContext.HttpContext.Request.Form[G_RESPONSE_FIELD_KEY];

            if ((!string.IsNullOrEmpty(captchaChallengeValue) && !string.IsNullOrEmpty(captchaResponseValue)) || !string.IsNullOrEmpty(gCaptchaResponseValue))
            {
                var captchaSettings = EngineContext.Current.Resolve<CaptchaSettings>();
                if (captchaSettings.Enabled)
                {
                    var captchaValidtor = new GReCaptchaValidator(captchaSettings.ReCaptchaVersion)
                    {
                        SecretKey = captchaSettings.ReCaptchaPrivateKey,
                        RemoteIp = filterContext.HttpContext.Request.UserHostAddress,
                        Response = captchaResponseValue ?? gCaptchaResponseValue,
                        Challenge = captchaChallengeValue
                    };

                    var recaptchaResponse = captchaValidtor.Validate();
                    valid = recaptchaResponse.IsValid;
                }
            }

            //这将在我们的操作中将结果值推送到一个参数中。
            filterContext.ActionParameters["captchaValid"] = valid;

            base.OnActionExecuting(filterContext);
        }
    }
}
