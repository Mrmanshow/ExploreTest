using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Game
{
    public enum GameType
    {
        /// <summary>
        /// 刮刮乐
        /// </summary>
        ScratchCard = 1,
        /// <summary>
        /// 深海狂鲨
        /// </summary>
        Sea = 2,
        /// <summary>
        /// 凿蛋
        /// </summary>
        LuckEgg = 3,
        /// <summary>
        /// 拉霸
        /// </summary>
        Laba = 4,
        /// <summary>
        /// 夺宝奇兵拉霸
        /// </summary>
        LabaNew = 5
    }
}
