using MyDouble;
using NIRS.H_Functions;
using NIRS.Nabla_Functions;
using NIRS.Parameter_names;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Interfaces
{
    public interface IProjectileFunctions
    {
        double Get(PN pn,double n);
        double Get_vSn(double n);
        double Get_x(double n);
        double Get_r(double n);
        double Get_z(double n);
        double Get_psi(double n);
        double Get_a(double n);
        double Get_m(double n);
        double Get_ro(double n);
        double Get_e(double n);
        double Get_p(double n);

        void Update(IGrid grid);
    }
}
