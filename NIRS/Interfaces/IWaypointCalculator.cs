using MyDouble;
using NIRS.Nabla_Functions.Projectile;
using NIRS.Parameter_names;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Interfaces
{
    public interface IWaypointCalculator
    {
        double Nabla(PN param1, PN param2, PN param3, LimitedDouble n, LimitedDouble k);
        double Nabla(PN param1, PN param2, LimitedDouble n, LimitedDouble k);
        double Nabla(PN param, LimitedDouble n, LimitedDouble k);
        double dPStrokeDivdx(LimitedDouble n, LimitedDouble k);

        IWaypointCalculatorProjectile sn { get; set; }

        void Update(IGrid grid);
    }
}
