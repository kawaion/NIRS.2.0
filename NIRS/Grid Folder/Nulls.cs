using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class Nulls
    {
        static double LimitedDoublNull = -100000000;
        public static LimitedDouble NullForN { get; } = new LimitedDouble(LimitedDoublNull);
        public static LimitedDouble NullForK { get; } = new LimitedDouble(LimitedDoublNull);
        public static double NullForVar { get; } = double.MinValue;
    }
}
