using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers
{
    /// <summary>
    /// 用户注册请求
    /// </summary>
    public class CustomerRegistrationRequest
    {

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="email">Email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password format</param>
        /// <param name="isApproved">Is approved</param>
        public CustomerRegistrationRequest(Customer customer, string username,
            string password,
            PasswordFormat passwordFormat,
            bool isApproved = true)
        {
            this.Customer = customer;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 密码格式
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }

        /// <summary>
        /// 是否接受注册
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
