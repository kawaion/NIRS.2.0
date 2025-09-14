using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System.Diagnostics;
using System.Threading.Tasks;
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
        public IGrid Get(IGrid grid, double n, double k)
        {
            if (ParameterTypeGetter.IsDynamic(n, k))
                grid = GetDynamicParametersOfNextLayer(grid, n, k, _functions);

            else if (ParameterTypeGetter.IsMixture(n, k))
                grid = GetMixtureParametersOfNextLayer(grid, n, k, _functions);

            return grid;
        }


        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, double n, double k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {

            grid[PN.dynamic_m, n, k] = _functions.Get_dynamic_m(n, k);
            grid[PN.M, n, k] = _functions.Get_M(n, k);
            grid[PN.v, n, k] = _functions.Get_v(n, k);
            grid[PN.w, n, k] = _functions.Get_w(n, k);
                

            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, double n, double k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            //if(n==1068 && k == 78.5)
            //{
            //    int c = 0;
            //}

            grid[PN.r, n, k] = functionsNewLayer.Get_r(n, k);
            grid[PN.a, n, k] = functionsNewLayer.Get_a(n, k);
            grid[PN.z, n, k] = functionsNewLayer.Get_z(n, k);
            grid[PN.psi, n, k] = functionsNewLayer.Get_psi(n, k);
            grid[PN.m, n, k] = functionsNewLayer.Get_m(n, k); // нужен a и psi
            grid[PN.rho, n, k] = functionsNewLayer.Get_rho(n, k); // может не занимать место в памяти, нужен r и m
            grid[PN.e, n, k] = functionsNewLayer.Get_e(n, k);// нужен z и psi
            grid[PN.p, n, k] = functionsNewLayer.Get_p(n, k);
            return grid;
        }
    }
}
