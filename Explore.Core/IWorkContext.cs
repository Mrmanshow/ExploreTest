using Explore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core
{
    /// <summary>
    /// Web应用程序的工作上下文
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// 获取或设置当前用户
        /// </summary>
        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// 获取或设置原始客户（在模拟当前客户的情况下）
        /// </summary>
        Customer OriginalCustomerIfImpersonated { get; }

        /// <summary>
        /// 获取或设置指示我们是否在管理区域的值
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
