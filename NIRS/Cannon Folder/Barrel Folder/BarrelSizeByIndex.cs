using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    static class BarrelSizeByIndex
    {
        private static double? h { get; set; } = null;
        public static double SByIndex(this IBarrelSize barrelSize, LimitedDouble k)
        {
            return barrelSize.S(k.Value * barrelSize.h);
        }
    }
}
