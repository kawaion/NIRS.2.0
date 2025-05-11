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
        double InterpolateMixture(PN pn, double n, double k);
        double InterpolateDynamic(PN pn, double n, double k);
        void Update(IGrid grid);
    }
}
