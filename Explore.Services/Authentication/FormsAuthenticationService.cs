using Explore.Core.Domain.Customers;
using Explore.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Explore.Services.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly TimeSpan _expirationTimeSpan;

        private Customer _cachedCustomer;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public FormsAuthenticationService(HttpContextBase httpContext,
            ICustomerService customerService)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获得认证客户
        /// </summary>
        /// <param name="ticket">票证</param>
        /// <returns>用户对象</returns>
        protected virtual Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var usernameOrEmail = ticket.UserData;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            var customer = _customerService.GetCustomerByUsername(usernameOrEmail);
            return customer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="customer">用户对象</param>
        /// <param name="createPersistentCookie">指示是否创建持久cookie的值</param>
        public virtual void SignIn(Customer customer, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                customer.Username,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                customer.Username,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedCustomer = customer;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public virtual void SignOut()
        {
            _cachedCustomer = null;
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 获得认证用户
        /// </summary>
        /// <returns>用户对象</returns>
        public virtual Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
                return _cachedCustomer;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            if (customer != null && customer.Active && !customer.RequireReLogin && !customer.Deleted && customer.IsAdmin())
                _cachedCustomer = customer;
            return _cachedCustomer;
        }

        #endregion

    }
}
