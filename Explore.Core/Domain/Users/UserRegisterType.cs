using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Core.Domain.Users
{
    public enum UserRegisterType
    {
        /// <summary>
        /// 手机
        /// </summary>
        Tel = 0,
        FaceBook = 1,
        Google = 2,
        /// <summary>
        /// 游客
        /// </summary>
        Guest = 3,
        Line =4 ,
        Twitter = 5
    }
}
