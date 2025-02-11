using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
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

        public IGrid Get(IGrid grid, LimitedDouble n)
        {
            if (ParameterTypeGetter.IsDynamic(n, k))
                grid = GetDynamicParametersOfNextLayer(grid, n, k, _functions);

            else if (ParameterTypeGetter.IsMixture(n, k))
                grid = GetMixtureParametersOfNextLayer(grid, n, k, _functions);

            return grid;
        }


        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            grid[n][k].D.dynamic_m = functionsNewLayer.Get(PN.dynamic_m, n, k);
            grid[n][k].D.v = functionsNewLayer.Get(PN.v, n, k);
            grid[n][k].D.M = functionsNewLayer.Get(PN.M, n, k);
            grid[n][k].D.w = functionsNewLayer.Get(PN.w, n, k);

            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            grid[n][k].M.r = functionsNewLayer.Get(PN.r, n, k);
            grid[n][k].M.e = functionsNewLayer.Get(PN.e, n, k);
            grid[n][k].M.psi = functionsNewLayer.Get(PN.psi, n, k);
            grid[n][k].M.z = functionsNewLayer.Get(PN.z, n, k);
            grid[n][k].M.a = functionsNewLayer.Get(PN.a, n, k);
            grid[n][k].M.p = functionsNewLayer.Get(PN.p, n, k);
            grid[n][k].M.m = functionsNewLayer.Get(PN.m, n, k);

            return grid;
        }
    }
}
