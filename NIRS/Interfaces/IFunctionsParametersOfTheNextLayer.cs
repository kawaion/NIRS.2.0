using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IFunctionsParametersOfTheNextLayer
    {
        double Get(PN pN, LimitedDouble n, LimitedDouble k);

        double Get_dynamic_m(LimitedDouble n, LimitedDouble k);
        double Get_v(LimitedDouble n, LimitedDouble k);
        double Get_M(LimitedDouble n, LimitedDouble k);
        double Get_w(LimitedDouble n, LimitedDouble k);
        double Get_r(LimitedDouble n, LimitedDouble k);
        double Get_e(LimitedDouble n, LimitedDouble k);
        double Get_psi(LimitedDouble n, LimitedDouble k);
        double Get_z(LimitedDouble n, LimitedDouble k);
        double Get_a(LimitedDouble n, LimitedDouble k);
        double Get_m(LimitedDouble n, LimitedDouble k);
        double Get_p(LimitedDouble n, LimitedDouble k);
        double Get_ro(LimitedDouble n, LimitedDouble k);
    }
}
