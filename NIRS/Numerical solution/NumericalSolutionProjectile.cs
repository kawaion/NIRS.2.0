using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_solution
{
    class NumericalSolutionProjectile : INumericalSolutionProjectile
    {
        private readonly IProjectileFunctions _functions;

        public NumericalSolutionProjectile(IProjectileFunctions projectileFunctions)
        {
            _functions = projectileFunctions;
        }

        public IGrid Get(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {
            if (n.Type == DoubleType.HalfInt)
                grid = GetDynamicParametersOfNextLayer(grid, n,  _functions, isBeltIntact);

            else if (n.Type == DoubleType.Int)
                grid = GetMixtureParametersOfNextLayer(grid, n,  _functions);

            return grid;
        }


        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, LimitedDouble n, IProjectileFunctions functions, bool isBeltIntact)
        {
            if (isBeltIntact)
                grid[n].sn.vSn = 0;
            else
                grid[n].sn.vSn = functions.Get(PN.v, n);

            grid[n].sn.x = functions.Get(PN.x, n); 

            grid[n].sn.dynamic_m = functions.Get(PN.dynamic_m, n);
            grid[n].sn.M = functions.Get(PN.M, n);
            grid[n].sn.w = functions.Get(PN.w, n);

            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, IProjectileFunctions functions)
        {
            grid[n].sn.x = functions.Get(PN.x, n);

            grid[n].sn.r = functions.Get(PN.r, n);
            grid[n].sn.e = functions.Get(PN.e, n);
            grid[n].sn.psi = functions.Get(PN.psi, n);
            grid[n].sn.z = functions.Get(PN.z, n);
            grid[n].sn.a = functions.Get(PN.a, n);
            grid[n].sn.p = functions.Get(PN.p, n);
            grid[n].sn.m = functions.Get(PN.m, n);

            return grid;
        }
    }
}
