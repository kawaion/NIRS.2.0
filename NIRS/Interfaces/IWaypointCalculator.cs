using MyDouble;
using NIRS.Nabla_Functions.Projectile;
using NIRS.Parameter_names;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Interfaces
{
    public interface IWaypointCalculator
    {
        double Nabla(PN param1, PN param2, PN param3, double n, double k);
        double Nabla(PN param1, PN param2, double n, double k);
        double Nabla(PN param, double n, double k);
        double dPStrokeDivdx(double n, double k);

        IWaypointCalculatorProjectile sn { get; set; }

        void Update(IGrid grid);
    }
}
