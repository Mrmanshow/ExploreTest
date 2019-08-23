using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Customers
{
    /// <summary>
    /// Customer logged-in event
    /// </summary>
    public class CustomerLoggedinEvent
    {
        public CustomerLoggedinEvent(Customer customer)
        {
            this.Customer = customer;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer
        {
            get;
            private set;
        }
    }
    /// <summary>
    /// “客户已注销”事件
    /// </summary>
    public class CustomerLoggedOutEvent
    {
        public CustomerLoggedOutEvent(Customer customer)
        {
            this.Customer = customer;
        }

        /// <summary>
        /// 获取或设置客户
        /// </summary>
        public Customer Customer { get; private set; }
    }

    /// <summary>
    /// Customer registered event
    /// </summary>
    public class CustomerRegisteredEvent
    {
        public CustomerRegisteredEvent(Customer customer)
        {
            this.Customer = customer;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 用户密码更改事件
    /// </summary>
    public class CustomerPasswordChangedEvent
    {
        public CustomerPasswordChangedEvent(CustomerPassword password)
        {
            this.Password = password;
        }

        /// <summary>
        /// 用户当前密码
        /// </summary>
        public CustomerPassword Password { get; private set; }
    }
}
