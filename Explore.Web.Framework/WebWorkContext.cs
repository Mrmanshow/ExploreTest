using Explore.Core;
using Explore.Core.Domain.Customers;
using Explore.Core.Fakes;
using Explore.Services;
using Explore.Services.Customers;
using Explore.Services.Common;
using Explore.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Explore.Services.Authentication;

namespace Explore.Web.Framework
{
    /// <summary>
    /// Web应用程序的工作上下文
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string CustomerCookieName = "Nop.customer";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserAgentHelper _userAgentHelper;

        private Customer _cachedCustomer;
        private Customer _originalCustomerIfImpersonated;

        #endregion

        #region Ctor

        public WebWorkContext(HttpContextBase httpContext,
            ICustomerService customerService,
            IAuthenticationService authenticationService,
            IUserAgentHelper userAgentHelper)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._authenticationService = authenticationService;
            this._userAgentHelper = userAgentHelper;
        }

        #endregion

        #region Utilities

        protected virtual HttpCookie GetCustomerCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[CustomerCookieName];
        }

        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(CustomerCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(CustomerCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置当前用户
        /// </summary>
        public virtual Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                Customer customer = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //检查是否由后台任务发出请求在这种情况下，返回后台任务的内置用户记录
                    customer = _customerService.GetCustomerBySystemName(SystemCustomerNames.BackgroundTask);
                }

                //检查搜索引擎是否发出请求，在这种情况下，返回搜索引擎的内置用户记录，或注释以下两行代码以禁用此功能
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    if (_userAgentHelper.IsSearchEngine())
                    {
                        customer = _customerService.GetCustomerBySystemName(SystemCustomerNames.SearchEngine);
                    }
                }

                //注册用户
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    customer = _authenticationService.GetAuthenticatedCustomer();
                }

                //如果需要 (目前用于支持'phone order')
                if (customer != null && !customer.Deleted && customer.Active && !customer.RequireReLogin)
                {
                    var impersonatedCustomerId = customer.GetAttribute<int?>(SystemCustomerAttributeNames.ImpersonatedCustomerId);
                    if (impersonatedCustomerId.HasValue && impersonatedCustomerId.Value > 0)
                    {
                        var impersonatedCustomer = _customerService.GetCustomerById(impersonatedCustomerId.Value);
                        if (impersonatedCustomer != null && !impersonatedCustomer.Deleted && impersonatedCustomer.Active && !impersonatedCustomer.RequireReLogin)
                        {
                            //设置模拟
                            _originalCustomerIfImpersonated = customer;
                            customer = impersonatedCustomer;
                        }
                    }
                }

                //加载游客
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    var customerCookie = GetCustomerCookie();
                    if (customerCookie != null && !String.IsNullOrEmpty(customerCookie.Value))
                    {
                        Guid customerGuid;
                        if (Guid.TryParse(customerCookie.Value, out customerGuid))
                        {
                            var customerByCookie = _customerService.GetCustomerByGuid(customerGuid);
                            if (customerByCookie != null &&
                                //不应注册此用户（来自cookie）
                                !customerByCookie.IsAdmin())
                                customer = customerByCookie;
                        }
                    }
                }

                //如果不存在则创建游客
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    customer = _customerService.InsertGuestCustomer();
                }


                //验证
                if (!customer.Deleted && customer.Active && !customer.RequireReLogin)
                {
                    SetCustomerCookie(customer.CustomerGuid);
                    _cachedCustomer = customer;
                }

                return _cachedCustomer;
            }
            set
            {
                SetCustomerCookie(value.CustomerGuid);
                _cachedCustomer = value;
            }
        }

        public virtual Customer OriginalCustomerIfImpersonated
        {
            get
            {
                return _originalCustomerIfImpersonated;
            }
        }

        /// <summary>
        /// 获取或设置指示我们是否在管理区域的值
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
