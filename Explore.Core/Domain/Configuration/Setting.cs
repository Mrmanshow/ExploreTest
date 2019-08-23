using Explore.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Configuration
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    public partial class Setting : BaseEntity, ILocalizedEntity
    {
        public Setting() { }

        public Setting(string name, string value, int storeId = 0)
        {
            this.Name = name;
            this.Value = value;
            this.StoreId = storeId;
        }

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 获取或设置此设置有效的存储。当设置为所有存储时设置0
        /// </summary>
        public int StoreId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
