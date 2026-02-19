using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface ICalculatorValuesInNodes
{
    public double Calculate(PN pn, LimitedDouble n, LimitedDouble k);


    public double Calculate_dynamic_m(LimitedDouble n, LimitedDouble k);
    public double GeCalculate_v(LimitedDouble n, LimitedDouble k);
    public double Calculate_M(LimitedDouble n, LimitedDouble k);
    public double Calculate_w(LimitedDouble n, LimitedDouble k);


    public double Calculate_r(LimitedDouble n, LimitedDouble k);
    public double Calculate_e(LimitedDouble N, LimitedDouble K);
    public double Calculate_psi(LimitedDouble n, LimitedDouble k);
    public double Calculate_z(LimitedDouble n, LimitedDouble k);
    double Calculate_a(LimitedDouble n, LimitedDouble k);
    double Calculate_p(LimitedDouble n, LimitedDouble k);
    double Calculate_m(LimitedDouble n, LimitedDouble k);







    double Calculate_rho(LimitedDouble n, LimitedDouble k);
}
