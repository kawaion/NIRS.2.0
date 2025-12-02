using MyDouble;
using NIRS.Barrel_Folder;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.H_Functions;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Numerical_solution
{
    class NumericalSolutionProjectile : INumericalSolutionProjectile
    {
        private readonly IProjectileFunctions _functions;

        private readonly double xEndChamber;
        private readonly IBarrelSize bs;

        public NumericalSolutionProjectile(IProjectileFunctions projectileFunctions, IMainData mainData)
        {
            _functions = projectileFunctions;
            xEndChamber = mainData.Barrel.EndChamberPoint.X;
            bs = mainData.Barrel.BarrelSize;
        }

        public IGrid Get(IGrid grid, LimitedDouble n)
        {
            if (n.IsHalfInt())
                grid = GetDynamicParametersOfNextLayer(grid, n,  _functions);

            grid.SetSn(PN.x, n,     _functions.Get(PN.x, n));

            if (n.IsInt())
                grid = GetMixtureParametersOfNextLayer(grid, n,  _functions);
                
            return grid;
        }
        public IGrid GetProjectileParametersBeforeBeltIntact(IGrid grid, LimitedDouble n)
        {
            grid.SetSn(PN.x, n,    xEndChamber);

            if (n.IsHalfInt())
            {
                grid.SetSn(PN.vSn, n,  1e-5);
            }
            else
            {
                var k = grid.LastIndexK(n);

                //grid.SetSn(PN.r, n,     grid[PN.r, n, k]);
                //
                grid.SetSn(PN.r, n,     grid[PN.rho, n, k] * grid[PN.m, n, k] * bs.Skm);
                //
                grid.SetSn(PN.e, n,     grid[PN.e, n, k]);
                grid.SetSn(PN.z, n,     grid[PN.z, n, k]);
                grid.SetSn(PN.psi, n,   grid[PN.psi, n, k]);
                grid.SetSn(PN.a, n,     grid[PN.a, n, k]);
                grid.SetSn(PN.m, n,     grid[PN.m, n, k]);
                grid.SetSn(PN.p, n,     grid[PN.p, n, k]);
                grid.SetSn(PN.rho, n,   grid[PN.rho, n, k]);
            }

            return grid;
        }

        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, LimitedDouble n, IProjectileFunctions functions)
        {
            grid.SetSn(PN.vSn, n,   functions.Get(PN.v, n));
            //grid[n].sn.dynamic_m = functions.GetNodeNK(PN.dynamic_m, n);
            //grid[n].sn.M = functions.GetNodeNK(PN.M, n);
            //grid[n].sn.w = functions.GetNodeNK(PN.w, n);

            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, IProjectileFunctions functions)
        {
            grid.SetSn(PN.r, n,     functions.Get(PN.r, n));
            grid.SetSn(PN.e, n,     functions.Get(PN.e, n));                
            grid.SetSn(PN.z, n,     functions.Get(PN.z, n));
            grid.SetSn(PN.psi, n,   functions.Get(PN.psi, n));
            grid.SetSn(PN.a, n,     functions.Get(PN.a, n));
            grid.SetSn(PN.m, n,     functions.Get(PN.m, n));
            grid.SetSn(PN.p, n,     functions.Get(PN.p, n));
            grid.SetSn(PN.rho, n,    functions.Get(PN.rho, n));

            return grid;
        }
    }
}
