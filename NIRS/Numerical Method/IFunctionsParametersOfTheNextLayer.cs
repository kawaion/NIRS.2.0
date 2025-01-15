using MyDouble;

namespace NIRS.Numerical_Method
{
    interface IFunctionsParametersOfTheNextLayer
    {
        double Calc_dynamic_m(LimitedDouble n, LimitedDouble k);
        double Calc_v(LimitedDouble n, LimitedDouble k);
        double Calc_M(LimitedDouble n, LimitedDouble k);
        double Calc_w(LimitedDouble n, LimitedDouble k);
        double Calc_r(LimitedDouble n, LimitedDouble k);
        double Calc_e(LimitedDouble n, LimitedDouble k);
        double Calc_psi(LimitedDouble n, LimitedDouble k);
        double Calc_z(LimitedDouble n, LimitedDouble k);
        double Calc_a(LimitedDouble n, LimitedDouble k);
        double Calc_m(LimitedDouble n, LimitedDouble k);
        double Calc_p(LimitedDouble n, LimitedDouble k);
        double Calc_ro(LimitedDouble n, LimitedDouble k);
    }
}
