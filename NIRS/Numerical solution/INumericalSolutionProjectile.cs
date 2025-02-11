using MyDouble;
using NIRS.Grid_Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_solution
{
    public interface INumericalSolutionProjectile
    {
        IGrid Get(IGrid grid, LimitedDouble n);
    }
}
