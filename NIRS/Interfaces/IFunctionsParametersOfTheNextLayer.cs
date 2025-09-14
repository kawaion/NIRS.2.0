using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IFunctionsParametersOfTheNextLayer
    {
        double Get(PN pN, double n, double k);

        double Get_dynamic_m(double n, double k);
        double Get_v(double n, double k);
        double Get_M(double n, double k);
        double Get_w(double n, double k);
        double Get_r(double n, double k);
        double Get_e(double n, double k);
        double Get_psi(double n, double k);
        double Get_z(double n, double k);
        double Get_a(double n, double k);
        double Get_m(double n, double k);
        double Get_p(double n, double k);
        double Get_rho(double n, double k);

        void Update(IGrid grid);
    }
}
