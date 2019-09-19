using Explore.Web.Framework;
using Explore.Web.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explore.Web.Models.Game
{
    public partial class GameLabaOrderModel : BaseExploreEntityModel
    {
        public string UserName { set; get; }

        public int Amount { set; get; }

        public int WinAmount { set; get; }

        public string Position { set; get; }

        public int FreeOrderId { set; get; }

        public int FreeCount { set; get; }

        public DateTime CreateTime { set; get; }

    }
}