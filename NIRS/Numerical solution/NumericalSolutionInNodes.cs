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
        public IGrid GetNodeNK(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            if (ParameterTypeGetter.IsDynamic(n, k))
                grid = GetDynamicParametersOfNextLayer(grid, n, k, _functions);

            else if (ParameterTypeGetter.IsMixture(n, k))
                grid = GetMixtureParametersOfNextLayer(grid, n, k, _functions);

            return grid;
        }


        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {

            double dynamic_m_value = _functions.Get_dynamic_m(n, k);
            grid[PN.dynamic_m, n, k] = dynamic_m_value;

            double M_value = _functions.Get_M(n, k);
            grid[PN.M, n, k] = M_value;

            double v_value = _functions.Get_v(n, k);
            grid[PN.v, n, k] = v_value;

            double w_value = _functions.Get_w(n, k);
            grid[PN.w, n, k] = w_value;


            return grid;
        }
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            //if(n==1068 && k == 78.5)
            //{
            //    int c = 0;
            //}

            double r_value = functionsNewLayer.Get_r(n, k);
            grid[PN.r, n, k] = r_value;

            double a_value = functionsNewLayer.Get_a(n, k);
            grid[PN.a, n, k] = a_value;

            double z_value = functionsNewLayer.Get_z(n, k);
            grid[PN.z, n, k] = z_value;

            double psi_value = functionsNewLayer.Get_psi(n, k);
            grid[PN.psi, n, k] = psi_value;

            double m_value = functionsNewLayer.Get_m(n, k); // нужен a и psi
            grid[PN.m, n, k] = m_value;

            double rho_value = functionsNewLayer.Get_rho(n, k); // нужен r и m
            grid[PN.rho, n, k] = rho_value;

            if (n == 1 && k == 0.5)
            {
                int c = 0;
            }
            double e_value = functionsNewLayer.Get_e(n, k); // нужен z и psi
            grid[PN.e, n, k] = e_value;

            double p_value = functionsNewLayer.Get_p(n, k);
            grid[PN.p, n, k] = p_value;

            return grid;
        }
    }
}
