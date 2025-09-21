using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDouble
{
    class HalfValueTransformer
    {
        public static int TransformForLimitedDouble(int integer, bool isFractional)
        {
            if ((integer < 0) && isFractional)
            {
                return integer - 1;
            }
            else
                return integer;
        }
        public static int TransformForInt(int integer, bool isFractional)
        {
            if ((integer < 0) && isFractional)
            {
                return integer + 1;
            }
            else
                return integer;
        }
    }
}
