using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDouble
{
    class ConverterOfCertainDouble
    {
        //static LimitedDouble d05 = new LimitedDouble(0, 5);
        //static LimitedDouble d1 = new LimitedDouble(1, 0);
        //static LimitedDouble d15 = new LimitedDouble(1, 5);

        //static LimitedDouble dMinus05 = new LimitedDouble(-0.5);
        //static LimitedDouble dMinus1 = new LimitedDouble(-1.0);
        //static LimitedDouble dMinus15 = new LimitedDouble(-1.5);

        //public LimitedDouble Convert (double value)
        //{
        //    if (value > 0)
        //    {
        //        if (Is05(value))
        //            return d05;
        //        if (Is1(value))
        //            return d1;
        //        if (Is15(value))
        //            return d15;
        //        throw new Exception();
        //    }
        //    else
        //    {
        //        if (Is05(-value))
        //            return dMinus05;
        //        if (Is1(-value))
        //            return dMinus1;
        //        if (Is15(-value))
        //            return dMinus15;
        //        throw new Exception();
        //    }
        //}

        //private const double eps = 1e-9;
        //public static bool Is05(double value)
        //{
        //    return 0.5 - eps < value && value < 0.5 + eps;
        //}
        //private static bool Is1(double value)
        //{
        //    return 1 - eps < value && value < 1 + eps;
        //}
        //private static bool Is15(double value)
        //{
        //    return 1.5 - eps < value && value < 1.5 + eps;
        //}
        private static readonly Dictionary<int, LimitedDouble> cache = new Dictionary<int, LimitedDouble>
        {
            [GetKey(0.5)] = new LimitedDouble(0, 5),
            [GetKey(1.0)] = new LimitedDouble(1, 0),
            [GetKey(1.5)] = new LimitedDouble(1, 5),
            [GetKey(-0.5)] = new LimitedDouble(-0.5),
            [GetKey(-1.0)] = new LimitedDouble(-1.0),
            [GetKey(-1.5)] = new LimitedDouble(-1.5)
        };

        private const int Scale = 2; // Для преобразования double в int

        public LimitedDouble Convert(double value)
        {
            if (cache.TryGetValue(GetKey(value), out var result))
            {
                return result;
            }

            throw new ArgumentException($"Value {value} is not one of the supported values");
        }

        private static int GetKey(double value)
        {
            return (int)(value * Scale);
        }
    }
}
