using MyDouble;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface IParameterInterpolationFunctions
    {
        double InterpolateMixture(PN pn, LimitedDouble n, double xSn, LimitedDouble kLast);
        double InterpolateDynamic(PN pn, LimitedDouble n, LimitedDouble kLast, double xSn);
        void Update(IGrid grid);
    }
}
