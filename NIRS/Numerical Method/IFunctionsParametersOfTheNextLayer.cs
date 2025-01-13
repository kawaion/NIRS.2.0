using System;
using NIRS.Grid_Folder;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_Method
{
    interface IFunctionsParametersOfTheNextLayer
    {
        double Calc_dynamic_m(double n, double k);
        double Calc_v(double n, double k);
        double Calc_M(double n, double k);
        double Calc_w(double n, double k);
        double Calc_r(double n, double k);
        double Calc_e(double n, double k);
        double Calc_psi(double n, double k);
        double Calc_z(double n, double k);
        double Calc_a(double n, double k);
        double Calc_m(double n, double k);
        double Calc_p(double n, double k);
        double Calc_ro(double n, double k);
    }
}
