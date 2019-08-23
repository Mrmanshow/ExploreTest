using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Web.Framework.Security.Captcha
{
    public class GReCaptchaResponse
    {
        public bool IsValid { get; set; }
        public List<string> ErrorCodes { get; set; }

        public GReCaptchaResponse()
        {
            ErrorCodes = new List<string>();
        }
    }
}
