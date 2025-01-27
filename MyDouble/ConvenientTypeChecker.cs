using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDouble
{
    public static class ConvenientTypeChecker
    {
        public static bool IsInt(this LimitedDouble limitedDouble)
        {
            if (limitedDouble.Type == DoubleType.Int) return true;
            else return false;
        }
        public static bool IsHalfInt(this LimitedDouble limitedDouble)
        {
            if (limitedDouble.Type == DoubleType.HalfInt) return true;
            else return false;
        }
    }
}
