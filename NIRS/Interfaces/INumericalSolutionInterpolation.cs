using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    internal interface INumericalSolutionInterpolation
    {
        IGrid GetAtUnavailableNodes(IGrid grid, LimitedDouble n);
    }
}
