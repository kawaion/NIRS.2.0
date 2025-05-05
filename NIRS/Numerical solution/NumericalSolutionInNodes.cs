using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System.Windows.Forms.VisualStyles;

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
            isNaN(grid[n][k].dynamic_m);
            grid[n][k].v = functionsNewLayer.Get(PN.v, n, k);
            isNaN(grid[n][k].v);
            grid[n][k].M = functionsNewLayer.Get(PN.M, n, k);
            isNaN(grid[n][k].M);
            grid[n][k].w = functionsNewLayer.Get(PN.w, n, k);
            isNaN(grid[n][k].w);

            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            grid[n][k].r = functionsNewLayer.Get(PN.r, n, k);
            isNaN(grid[n][k].r);
            grid[n][k].a = functionsNewLayer.Get(PN.a, n, k);
            isNaN(grid[n][k].a);
            grid[n][k].psi = functionsNewLayer.Get(PN.psi, n, k);
            isNaN(grid[n][k].psi);
            grid[n][k].z = functionsNewLayer.Get(PN.z, n, k);
            isNaN(grid[n][k].z);
            grid[n][k].m = functionsNewLayer.Get(PN.m, n, k); // нужен a и psi
            isNaN(grid[n][k].m);
            grid[n][k].ro = functionsNewLayer.Get(PN.ro, n, k); // может не занимать место в памяти, нужен r и m
            isNaN(grid[n][k].ro);
            grid[n][k].e = functionsNewLayer.Get(PN.e, n, k);// нужен z и psi
            isNaN(grid[n][k].e);
            grid[n][k].p = functionsNewLayer.Get(PN.p, n, k);
            isNaN(grid[n][k].p);

            return grid;
        }
        private bool isNaN(double value)
        {
            if (double.IsNaN(value))
            {
                return true;
            }
            else return false;
        }
    }
}
