using MyDouble;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.H_Functions;
using NIRS.Nabla_Functions;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_Method
{
    class NumericalSolutionInNodes : INumericalSolutionInNodes
    {
        private IFunctionsParametersOfTheNextLayer _functionsNewLayer;
        public NumericalSolutionInNodes(IFunctionsParametersOfTheNextLayer functionsParametersOfTheNextLayer)
        {
            _functionsNewLayer = functionsParametersOfTheNextLayer;
        }
        public IGrid Get(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            if (ParameterTypeGetter.IsDynamic(n, k))
                grid = GetDynamicParametersOfNextLayer(grid, n, k, _functionsNewLayer);

            else if (ParameterTypeGetter.IsMixture(n, k))
                grid = GetMixtureParametersOfNextLayer(grid, n, k, _functionsNewLayer);

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
