using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Content
{
    public partial class Banner : BaseEntity
    {
        #region Properties

        public string BannerImg { get; set; }

        public string Theme { get; set; }

        public int Type { get; set; }

        public string BannerLink { get; set; }

        public int BannerOrder { get; set; }

        public DateTime ShowBeginDate { get; set; }

        public DateTime ShowEndDate { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }

        #endregion

        #region Custom properties


        /// <summary>
        /// 获取或设置Banner状态
        /// </summary>
        public BannerStatus BannerStatus
        {
            get
            {
                return (BannerStatus)this.Status;
            }
            set
            {
                this.Status = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置Banner出现位置
        public BannerType BannerType
        {
            get
            {
                return (BannerType)this.Type;
            }
            set
            {
                this.Status = (int)value;
            }
        }

        #endregion
    }
}
