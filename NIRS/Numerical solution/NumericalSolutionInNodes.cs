﻿using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;

namespace NIRS.Numerical_solution
{
    class NumericalSolutionInNodes : INumericalSolutionInNodes
    {
        private IFunctionsParametersOfTheNextLayer _functions;
        public NumericalSolutionInNodes(IFunctionsParametersOfTheNextLayer functionsParametersOfTheNextLayer)
        {
            _functions = functionsParametersOfTheNextLayer;
        }
        public IGrid Get(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            if (ParameterTypeGetter.IsDynamic(n, k))
                grid = GetDynamicParametersOfNextLayer(grid, n, k, _functions);

            else if (ParameterTypeGetter.IsMixture(n, k))
                grid = GetMixtureParametersOfNextLayer(grid, n, k, _functions);

            return grid;
        }


        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            grid[n][k].dynamic_m = functionsNewLayer.Get(PN.dynamic_m, n, k);
            grid[n][k].v = functionsNewLayer.Get(PN.v, n, k);
            grid[n][k].M = functionsNewLayer.Get(PN.M, n, k);
            grid[n][k].w = functionsNewLayer.Get(PN.w, n, k);

            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            grid[n][k].r = functionsNewLayer.Get(PN.r, n, k);
            grid[n][k].e = functionsNewLayer.Get(PN.e, n, k);
            grid[n][k].psi = functionsNewLayer.Get(PN.psi, n, k);
            grid[n][k].z = functionsNewLayer.Get(PN.z, n, k);
            grid[n][k].a = functionsNewLayer.Get(PN.a, n, k);
            grid[n][k].p = functionsNewLayer.Get(PN.p, n, k);
            grid[n][k].m = functionsNewLayer.Get(PN.m, n, k);

            return grid;
        }
    }
}
