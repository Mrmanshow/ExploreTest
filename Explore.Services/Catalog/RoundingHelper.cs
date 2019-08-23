using Explore.Core;
using Explore.Core.Domain.Directory;
using Explore.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Catalog
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class RoundingHelper
    {

        /// <summary>
        /// Round
        /// </summary>
        /// <param name="value">Value to round</param>
        /// <param name="roundingType">The rounding type</param>
        /// <returns>Rounded value</returns>
        public static decimal Round(this decimal value, RoundingType roundingType)
        {
            //default round (Rounding001)
            var rez = Math.Round(value, 2);
            decimal t;

            //Cash rounding (details: https://en.wikipedia.org/wiki/Cash_rounding)
            switch (roundingType)
            {
                //以0.05或5个间隔舍入
                case RoundingType.Rounding005Up:
                case RoundingType.Rounding005Down:
                    t = (rez - Math.Truncate(rez)) * 10;
                    t = (t - Math.Truncate(t)) * 10;

                    if (roundingType == RoundingType.Rounding005Down)
                        t = t >= 5 ? 5 - t : t * -1;
                    else
                        t = t >= 5 ? 10 - t : 5 - t;

                    rez += t / 100;
                    break;
                //以0.10个间隔舍入
                case RoundingType.Rounding01Up:
                case RoundingType.Rounding01Down:
                    t = (rez - Math.Truncate(rez)) * 10;
                    t = (t - Math.Truncate(t)) * 10;

                    if (roundingType == RoundingType.Rounding01Down && t == 5)
                        t = -5;
                    else
                        t = t < 5 ? t * -1 : 10 - t;

                    rez += t / 100;
                    break;
                //以0.50间隔舍入
                case RoundingType.Rounding05:
                    t = (rez - Math.Truncate(rez)) * 100;
                    t = t < 25 ? t * -1 : t < 50 || t < 75 ? 50 - t : 100 - t;

                    rez += t / 100;
                    break;
                //以1.00间隔舍入
                case RoundingType.Rounding1:
                case RoundingType.Rounding1Up:
                    t = (rez - Math.Truncate(rez)) * 100;

                    if (roundingType == RoundingType.Rounding1Up && t > 0)
                        rez = Math.Truncate(rez) + 1;
                    else
                        rez = t < 50 ? Math.Truncate(rez) : Math.Truncate(rez) + 1;

                    break;
                case RoundingType.Rounding001:
                default:
                    break;
            }

            return rez;
        }
    }
}
