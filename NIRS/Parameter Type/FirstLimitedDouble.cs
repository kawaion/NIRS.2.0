using MyDouble;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Parameter_Type
{
    static class FirstLimitedDouble
    {
        static LimitedDouble d0 = new LimitedDouble(0);
        static LimitedDouble d05 = new LimitedDouble(0.5);
        public static LimitedDouble GetFirstN(PN pn)
        {
            if (ParameterTypeGetter.IsDynamic(pn))
                return d05;
            if (ParameterTypeGetter.IsMixture(pn))
                return d0;
            throw new Exception();
        }
        public static LimitedDouble GetFirstK(PN pn)
        {
            if (ParameterTypeGetter.IsDynamic(pn))
                return d0;
            if (ParameterTypeGetter.IsMixture(pn))
                return d05;
            throw new Exception();
        }
    }
}
