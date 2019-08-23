using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers
{
    /// <summary>
    /// 用户注册结果
    /// </summary>
    public class CustomerRegistrationResult
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public CustomerRegistrationResult()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// 获取一个值，该值指示请求是否已成功完成
        /// </summary>
        public bool Success
        {
            get { return !this.Errors.Any(); }
        }

        /// <summary>
        /// 添加错误
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        /// <summary>
        /// Errors
        /// </summary>
        public IList<string> Errors { get; set; }
    }
}
