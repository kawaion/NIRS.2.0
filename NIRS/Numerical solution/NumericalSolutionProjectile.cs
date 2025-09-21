using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.Helpers;
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
            if (n.IsHalfInt())
                grid = GetDynamicParametersOfNextLayer(grid, n,  _functions, isBeltIntact);

            grid.SetSn(PN.x, n,     _functions.Get(PN.x, n));

            if (n.IsInt())
                grid = GetMixtureParametersOfNextLayer(grid, n,  _functions, isBeltIntact);
                
            
             

            return grid;
        }


        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, LimitedDouble n, IProjectileFunctions functions, bool isBeltIntact)
        {
            if (isBeltIntact)
                grid.SetSn(PN.vSn, n,   0);
            else
                grid.SetSn(PN.vSn, n,   functions.Get(PN.v, n));
            //grid[n].sn.dynamic_m = functions.Get(PN.dynamic_m, n);
            //grid[n].sn.M = functions.Get(PN.M, n);
            //grid[n].sn.w = functions.Get(PN.w, n);

            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, IProjectileFunctions functions, bool isBeltIntact)
        {
            if (!isBeltIntact)
            {
                grid.SetSn(PN.r, n,     functions.Get(PN.r, n));
                grid.SetSn(PN.e, n,     functions.Get(PN.e, n));                
                grid.SetSn(PN.z, n,     functions.Get(PN.z, n));
                grid.SetSn(PN.psi, n,   functions.Get(PN.psi, n));
                grid.SetSn(PN.a, n,     functions.Get(PN.a, n));
                grid.SetSn(PN.m, n,     functions.Get(PN.m, n));
                grid.SetSn(PN.p, n,     functions.Get(PN.p, n));
                grid.SetSn(PN.rho, n,    functions.Get(PN.rho, n));
            }

            return grid;
        }
    }
}
