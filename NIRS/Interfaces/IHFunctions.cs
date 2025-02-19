using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDouble;

namespace NIRS.Interfaces
{
    public interface IHFunctions
    {
        double H1(LimitedDouble n, LimitedDouble k);
        double H2(LimitedDouble n, LimitedDouble k);
        double H3(LimitedDouble n, LimitedDouble k);
        double H4(LimitedDouble n, LimitedDouble k);
        double H5(LimitedDouble n, LimitedDouble k);
        double HPsi(LimitedDouble n, LimitedDouble k);

        IHFunctionsProjectile sn { get; set; }
    }
}
