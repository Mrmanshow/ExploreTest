using Explore.Core;
using Explore.Core.Domain.Customers;
using Explore.Services.Authentication;
using Explore.Services.Authentication.External;
using Explore.Services.Common;
using Explore.Services.Customers;
using Explore.Services.Events;
using Explore.Services.Localization;
using Explore.Services.Logging;
using Explore.Services.Security;
using Explore.Services.Users;
using Explore.Web.Factories;
using Explore.Web.Framework.Kendoui;
using Explore.Web.Framework.Security;
using Explore.Web.Framework.Security.Captcha;
using Explore.Web.Models.Customer;
using Explore.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Explore.Web.Controllers
{
    public partial class UserController : Controller
    {

        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerModelFactory _customerModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IWebHelper _webHelper;
        private readonly CustomerSettings _customerSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IUsersService _usersService;


        #endregion

        #region Ctor

        public UserController(IAuthenticationService authenticationService,
            ICustomerModelFactory customerModelFactory,
            IWorkContext workContext,
            ICustomerService customerService,
            ICustomerRegistrationService customerRegistrationService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            CustomerSettings customerSettings,
            ICustomerActivityService customerActivityService,
            IEventPublisher eventPublisher,
            CaptchaSettings captchaSettings,
            IPermissionService permissionService,
            IUsersService usersService)
        {
            this._authenticationService = authenticationService;
            this._customerModelFactory = customerModelFactory;
            this._workContext = workContext;
            this._customerService = customerService;
            this._customerRegistrationService = customerRegistrationService;
            this._webHelper = webHelper;
            this._customerSettings = customerSettings;
            this._captchaSettings = captchaSettings;
            this._eventPublisher = eventPublisher;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._genericAttributeService = genericAttributeService;
            this._permissionService = permissionService;
            this._usersService = usersService;
        }

        #endregion


        #region Utilities


        #endregion

        // GET: User
        #region Register

        public virtual ActionResult Register()
        {
            var model = new RegisterModel();
            //model = _customerModelFactory.PrepareRegisterModel(model, false, setDefaultValues: true);

            return View(model);
        }

        [HttpPost]
        [CaptchaValidator]
        [ValidateInput(false)]
        public virtual ActionResult Register(RegisterModel model, string returnUrl, bool captchaValid, FormCollection form)
        {
            //检查是否允许注册
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            if (_workContext.CurrentCustomer.IsAdmin())
            {
                //已注册
                _authenticationService.SignOut();

                //保存新记录
                _workContext.CurrentCustomer = _customerService.InsertGuestCustomer();
            }
            var customer = _workContext.CurrentCustomer;

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage && !captchaValid)
            {
                //ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                if (model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }

                bool isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new CustomerRegistrationRequest(customer,
                    model.Username,
                    model.Password,
                     _customerSettings.DefaultPasswordFormat,
                    isApproved);
                var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                if (registrationResult.Success)
                {
                    //login customer now
                    if (isApproved)
                        _authenticationService.SignIn(customer, true);


                    switch (_customerSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.EmailValidation:
                        {
                            //email validation message
                            //_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
                            //_workflowMessageService.SendCustomerEmailValidationMessage(customer, _workContext.WorkingLanguage.Id);

                            //result
                            return RedirectToRoute("RegisterResult",
                                new { resultId = (int)UserRegistrationType.EmailValidation });
                        }
                        case UserRegistrationType.AdminApproval:
                        {
                            return RedirectToRoute("RegisterResult",
                                new { resultId = (int)UserRegistrationType.AdminApproval });
                        }
                        case UserRegistrationType.Standard:
                        {
                            //send customer welcome message
                            //_workflowMessageService.SendCustomerWelcomeMessage(customer, _workContext.WorkingLanguage.Id);

                            var redirectUrl = Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                                redirectUrl = _webHelper.ModifyQueryString(redirectUrl, "returnurl=" + HttpUtility.UrlEncode(returnUrl), null);
                            return Redirect(redirectUrl);
                        }
                        default:
                        {
                            return RedirectToRoute("HomePage");
                        }
                    }


                }

                //errors
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("Username", error);
            }

            //If we got this far, something failed, redisplay form
            //model = _customerModelFactory.PrepareRegisterModel(model, true, customerAttributesXml);
            return View();
        }

        //available even when navigation is not allowed
        //[PublicStoreAllowNavigation(true)]
        public virtual ActionResult RegisterResult(int resultId)
        {
            //var model = _customerModelFactory.PrepareRegisterResultModel(resultId);
            return View();
        }

        //available even when navigation is not allowed
        //[PublicStoreAllowNavigation(true)]
        [HttpPost]
        public virtual ActionResult RegisterResult(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToRoute("HomePage");

            return Redirect(returnUrl);
        }



        #endregion

        #region Login / logout

        //[ExploreHttpsRequirement(SslRequirement.Yes)]
        public virtual ActionResult Login(bool? checkoutAsGuest)
        {
            var model = _customerModelFactory.PrepareLoginModel(checkoutAsGuest);
            return View(model);
        }

        [HttpPost]
        [CaptchaValidator]
        public virtual ActionResult Login(LoginModel model, string returnUrl, bool captchaValid)
        {

            //validate CAPTCHA
            //if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            //{
            //    ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            //}

            if (ModelState.IsValid)
            {
                model.Username = model.Username.Trim();

                var loginResult = _customerRegistrationService.ValidateCustomer(model.Username, model.Password);

                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            var customer = _customerService.GetCustomerByUsername(model.Username);


                            //登录新客户
                            _authenticationService.SignIn(customer, model.RememberMe);

                            //触发事件  
                            _eventPublisher.Publish(new CustomerLoggedinEvent(customer));

                            //activity log
                            _customerActivityService.InsertActivity(customer, "PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"));

                            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToRoute("HomePage");

                            return Redirect(returnUrl);
                        }
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist"));
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.Deleted"));
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered"));
                        break;
                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut"));
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            model = _customerModelFactory.PrepareLoginModel(model.CheckoutAsGuest);
            return View(model);
        }

        public virtual ActionResult Logout()
        {
            //外部身份验证
            ExternalAuthorizerHelper.RemoveParameters();

            if (_workContext.OriginalCustomerIfImpersonated != null)
            {
                //activity log
                _customerActivityService.InsertActivity(_workContext.OriginalCustomerIfImpersonated,
                    "Impersonation.Finished",
                    _localizationService.GetResource("ActivityLog.Impersonation.Finished.StoreOwner"),
                    _workContext.CurrentCustomer.Username, _workContext.CurrentCustomer.Id);
                _customerActivityService.InsertActivity("Impersonation.Finished",
                    _localizationService.GetResource("ActivityLog.Impersonation.Finished.Customer"),
                    _workContext.OriginalCustomerIfImpersonated.Username, _workContext.OriginalCustomerIfImpersonated.Id);

                //logout impersonated customer
                _genericAttributeService.SaveAttribute<int?>(_workContext.OriginalCustomerIfImpersonated,
                    SystemCustomerAttributeNames.ImpersonatedCustomerId, null);

                //redirect back to customer details page (admin area)
                return this.RedirectToAction("Edit", "Customer",
                    new { id = _workContext.CurrentCustomer.Id, area = "Admin" });

            }

            //活动日志
            _customerActivityService.InsertActivity("PublicStore.Logout", _localizationService.GetResource("ActivityLog.PublicStore.Logout"));

            //标准注销
            _authenticationService.SignOut();

            //引发注销事件    
            _eventPublisher.Publish(new CustomerLoggedOutEvent(_workContext.CurrentCustomer));

            //EU Cookie
            //if (_storeInformationSettings.DisplayEuCookieLawWarning)
            //{
            //    //the cookie law message should not pop up immediately after logout.
            //    //otherwise, the user will have to click it again...
            //    //and thus next visitor will not click it... so violation for that cookie law..
            //    //the only good solution in this case is to store a temporary variable
            //    //indicating that the EU cookie popup window should not be displayed on the next page open (after logout redirection to homepage)
            //    //but it'll be displayed for further page loads
            //    TempData["nop.IgnoreEuCookieLawWarning"] = true;
            //}

            return RedirectToRoute("Login");
        }

        #endregion
    }
}