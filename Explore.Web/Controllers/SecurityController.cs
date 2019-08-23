using Explore.Core;
using Explore.Services.Customers;
using Explore.Services.Localization;
using Explore.Services.Logging;
using Explore.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Controllers
{
    public partial class SecurityController : BaseAdminController
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors

        public SecurityController(ILogger logger, IWorkContext workContext,
            IPermissionService permissionService,
            ICustomerService customerService, ILocalizationService localizationService)
        {
            this._logger = logger;
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._customerService = customerService;
            this._localizationService = localizationService;
        }

        #endregion

        #region Methods

        public virtual ActionResult AccessDenied(string pageUrl)
        {
            var currentCustomer = _workContext.CurrentCustomer;
            if (currentCustomer == null)
            {
                _logger.Information(string.Format("拒绝匿名请求的访问 {0}", pageUrl));
                return View();
            }

            _logger.Information(string.Format("拒绝用户访问 #{0} '{1}' on {2}", currentCustomer.Username, currentCustomer.Username, pageUrl));


            return View();
        }
        #endregion
    }
}