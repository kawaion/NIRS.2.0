using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDouble
{
    class ConverterToLimitedDoubleValue
    {
        public (int,bool) LimitedDoubleConverter(double value)
        {
            var intValue = (int)(value * 2);
            return (intValue, GetHalf(intValue));
        }
        public (int, bool) LimitedDoubleConverter(int value)
        {
            return (value * 2, false);
        }
        private bool GetHalf(int value)
        {
            return value % 2 != 0;
        }
    }
}
