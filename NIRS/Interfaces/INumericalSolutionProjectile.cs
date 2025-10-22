using MyDouble;
using NIRS.Grid_Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface INumericalSolutionProjectile
    {
        IGrid Get(IGrid grid, LimitedDouble n);
        IGrid GetProjectileParametersBeforeBeltIntact(IGrid grid, LimitedDouble n);
    }
}
