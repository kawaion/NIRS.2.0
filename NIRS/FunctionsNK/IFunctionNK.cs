using MyDouble;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.FunctionsNK
{
    public interface IFunctionNK
    {
        double Get(PN pn, LimitedDouble n, LimitedDouble k);
    }
}
