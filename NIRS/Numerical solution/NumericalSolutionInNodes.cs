using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System.Diagnostics;
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            grid[PN.dynamic_m, n, k] = functionsNewLayer.Get_dynamic_m(n, k);
            var tmp1 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.v, n, k] = functionsNewLayer.Get_v(n, k);
            var tmp2 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.M, n, k] = functionsNewLayer.Get_M(n, k);
            var tmp3 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.w, n, k] = functionsNewLayer.Get_w(n, k);
            var tmp4 = stopwatch.Elapsed;
            stopwatch.Restart();
            stopwatch.Reset();
            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            grid[PN.r, n, k] = functionsNewLayer.Get_r(n, k);
            var tmp1 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.a, n, k] = functionsNewLayer.Get_a(n, k);
            var tmp2 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.psi, n, k] = functionsNewLayer.Get_psi(n, k);
            var tmp3 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.z, n, k] = functionsNewLayer.Get_z(n, k);
            var tmp4 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.m, n, k] = functionsNewLayer.Get_m(n, k); // нужен a и psi
            var tmp5 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.ro, n, k] = functionsNewLayer.Get_ro(n, k); // может не занимать место в памяти, нужен r и m
            var tmp6 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.e, n, k] = functionsNewLayer.Get_e(n, k);// нужен z и psi
            var tmp7 = stopwatch.Elapsed;
            stopwatch.Restart();
            grid[PN.p, n, k] = functionsNewLayer.Get_p(n, k);
            var tmp8 = stopwatch.Elapsed;
            stopwatch.Restart();
            stopwatch.Reset();
            return grid;
        }
    }
}
