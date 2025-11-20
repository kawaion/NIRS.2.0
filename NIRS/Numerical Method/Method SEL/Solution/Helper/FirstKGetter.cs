using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_Method.Method_SEL.Solution
{
    static class FirstKGetter
    {
        public static LimitedDouble Get05_or_1(LimitedDouble n)
        {
            if (n.IsInt())
                return new LimitedDouble(0.5);
            else
                return new LimitedDouble(1);
        }
    }
}
