using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game
{
    public partial class GameTurnover: BaseEntity
    {
        public string CreateTime { get; set; }

        public GameType GameType { set; get; }

        public int GameUser { set; get; }

        public int GameCount { set; get; }

        public decimal GameWin { set; get; }

        public decimal GameFail { set; get; }
    }
}
