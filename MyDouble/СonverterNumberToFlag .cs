using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDouble
{
    class СonverterNumberToFlag
    {
        public static bool Convert(int fractional)
        {
            if (fractional == 0)
                return false;
            else if (fractional == 5)
                return true;
            else
                throw new Exception();
        }
    }
}
