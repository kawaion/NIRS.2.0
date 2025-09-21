using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface IHFunctionsProjectile
    {
        double H3(LimitedDouble n);
        double H4(LimitedDouble n);
        double H5(LimitedDouble n);
        double HPsi(LimitedDouble n);
    }
}
