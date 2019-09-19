﻿using Explore.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game.Laba
{
    public partial class LabaOrder: BaseEntity
    {
        public int WinAmount { set; get; }

        public int Amount { set; get; }

        public int UserId { set; get; }

        public int Status { set; get; }

        public string Position { set; get; }

        public DateTime CreateTime { set; get; }

        public virtual User User { set; get; }
    }
}
