using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game.Laba
{
    public partial class LabaWinRouteNew : BaseEntity
    {
        public int X1 { set; get; }

        public int X2 { set; get; }

        public int X3 { set; get; }

        public int X4 { set; get; }

        public int X5 { set; get; }

        public int Y1 { set; get; }

        public int Y2 { set; get; }

        public int Y3 { set; get; }

        public int Y4 { set; get; }

        public int Y5 { set; get; }

        public int Status { set; get; }

        public int Sequence { set; get; }

        public DateTime CreateTime { set; get; }

        #region Custom properties

        public LabaRouteStatus LabaRouteStatus
        {
            get { return (LabaRouteStatus)this.Status; }
            set { this.Status = (int)value; }
        }

        #endregion
    }
}
